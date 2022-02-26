using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Examine;
using Examine.Lucene.Search;
using Examine.Search;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Facet;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Search.Highlight;
using Lucene.Net.Util;

namespace SImpl.SearchModule.Examine.Application.LuceneEngine
{

    /// <summary>
    /// An implementation of the search results returned from Lucene.Net
    /// </summary>
    public class LuceneSearchExecutorWithFacetsAndFilters
    {
        private readonly QueryOptions _options;
        private readonly IEnumerable<SortField> _sortField;
        private readonly ISearchContext _searchContext;
        private readonly Query _luceneQuery;
        private readonly Filter _filter;
        private readonly Dictionary<string, Query> _highlitherQueries;
        private readonly ISet<string> _fieldsToLoad;
        private int? _maxDoc;

        internal LuceneSearchExecutorWithFacetsAndFilters(QueryOptions options, Query query, Filter filter, Dictionary<string, Query> highlitherQueries,
            IEnumerable<SortField> sortField, ISearchContext searchContext, ISet<string> fieldsToLoad)
        {
            _options = options ?? QueryOptions.Default;
            _luceneQuery = query ?? throw new ArgumentNullException(nameof(query));
            _filter = filter;
            _highlitherQueries = highlitherQueries;
            _fieldsToLoad = fieldsToLoad;
            _sortField = sortField ?? throw new ArgumentNullException(nameof(sortField));
            _searchContext = searchContext ?? throw new ArgumentNullException(nameof(searchContext));
        }

        private int MaxDoc
        {
            get
            {
                if (_maxDoc == null)
                {
                    using (ISearcherReference searcher = _searchContext.GetSearcher())
                    {
                        _maxDoc = searcher.IndexSearcher.IndexReader.MaxDoc;
                    }
                }
                return _maxDoc.Value;
            }
        }

        public ISearchResults Execute()
        {
            var extractTermsSupported = CheckQueryForExtractTerms(_luceneQuery);

            if (extractTermsSupported)
            {
                //This try catch is because analyzers strip out stop words and sometimes leave the query
                //with null values. This simply tries to extract terms, if it fails with a null
                //reference then its an invalid null query, NotSupporteException occurs when the query is
                //valid but the type of query can't extract terms.
                //This IS a work-around, theoretically Lucene itself should check for null query parameters
                //before throwing exceptions.
                try
                {
                    var set = new HashSet<Term>();
                    _luceneQuery.ExtractTerms(set);
                }
                catch (NullReferenceException)
                {
                    //this means that an analyzer has stipped out stop words and now there are
                    //no words left to search on

                    //it could also mean that potentially a IIndexFieldValueType is throwing a null ref
                    return LuceneSearchResults.Empty;
                }
                catch (NotSupportedException)
                {
                    //swallow this exception, we should continue if this occurs.
                }
            }

            var maxResults = Math.Min((_options.Skip + 1) * _options.Take, MaxDoc);
            maxResults = maxResults >= 1 ? maxResults : QueryOptions.DefaultMaxResults;

            ICollector topDocsCollector;
            SortField[] sortFields = _sortField as SortField[] ?? _sortField.ToArray();
            if (sortFields.Length > 0)
            {
                topDocsCollector = TopFieldCollector.Create(
                    new Sort(sortFields), maxResults, false, false, false, false);
            }
            else
            {
                topDocsCollector = TopScoreDocCollector.Create(maxResults, true);
            }

            FacetsCollector facetsCollector = new FacetsCollector(true);
            using (ISearcherReference searcher = _searchContext.GetSearcher())
            {
                //todo: figure out facet fields
            
                if (_filter == null)
                {
                    searcher.IndexSearcher.Search(_luceneQuery, topDocsCollector);
  
                }
                else
                {
                    searcher.IndexSearcher.Search(_luceneQuery,_filter, topDocsCollector);
                }
                TopDocs topDocs;
                if (sortFields.Length > 0)
                {
                    topDocs = ((TopFieldCollector)topDocsCollector).GetTopDocs(_options.Skip, _options.Take);
                }
                else
                {
                    topDocs = ((TopScoreDocCollector)topDocsCollector).GetTopDocs(_options.Skip, _options.Take);
                }

                var totalItemCount = topDocs.TotalHits;

                var results = new List<ISearchResult>();
            
              
                for (int i = 0; i < topDocs.ScoreDocs.Length; i++)
                {
                    var result = GetSearchResult(i, topDocs, searcher.IndexSearcher);
                    foreach (var query in _highlitherQueries)
                    {
                        QueryScorer scorer = new QueryScorer(query.Value);
                        SimpleHTMLFormatter formatter = new SimpleHTMLFormatter("em", "em");
                        Highlighter highlighter = new Highlighter(formatter, scorer);
                        highlighter.TextFragmenter = new SimpleFragmenter();
                        TokenStream stream = new StandardAnalyzer(LuceneVersion.LUCENE_48).GetTokenStream(query.Key,result[query.Key] );
                        highlighter.GetBestFragments(stream,result[query.Key],int.MaxValue);
                        results.Add(result);
                    }
                  
                }
                
            
                return new LuceneSearchResults(results, totalItemCount);
            }
        }

        private ISearchResult GetSearchResult(int index, TopDocs topDocs, IndexSearcher luceneSearcher)
        {
            // I have seen IndexOutOfRangeException here which is strange as this is only called in one place
            // and from that one place "i" is always less than the size of this collection. 
            // but we'll error check here anyways
            if (topDocs?.ScoreDocs.Length < index)
            {
                return null;
            }

            var scoreDoc = topDocs.ScoreDocs[index];

            var docId = scoreDoc.Doc;
            Document doc;
            if (_fieldsToLoad != null)
            {
                doc = luceneSearcher.Doc(docId, _fieldsToLoad);
            }
            else
            {
                doc = luceneSearcher.Doc(docId);
            }
            var score = scoreDoc.Score;
            var result = CreateSearchResult(doc, score);

            return result;
        }

        /// <summary>
        /// Creates the search result from a <see cref="Lucene.Net.Documents.Document"/>
        /// </summary>
        /// <param name="doc">The doc to convert.</param>
        /// <param name="score">The score.</param>
        /// <returns>A populated search result object</returns>
        private ISearchResult CreateSearchResult(Document doc, float score)
        {
            var id = doc.Get("id");

            if (string.IsNullOrEmpty(id) == true)
            {
                id = doc.Get(ExamineFieldNames.ItemIdFieldName);
            }

            var searchResult = new SearchResult(id, score, () =>
            {
                //we can use lucene to find out the fields which have been stored for this particular document
                var fields = doc.Fields;

                var resultVals = new Dictionary<string, List<string>>();

                foreach (var field in fields.Cast<Field>())
                {
                    var fieldName = field.Name;
                    var values = doc.GetValues(fieldName);

                    if (resultVals.TryGetValue(fieldName, out var resultFieldVals))
                    {
                        foreach (var value in values)
                        {
                            if (!resultFieldVals.Contains(value))
                            {
                                resultFieldVals.Add(value);
                            }
                        }
                    }
                    else
                    {
                        resultVals[fieldName] = values.ToList();
                    }
                }

                return resultVals;
            });

            return searchResult;
        }

        private bool CheckQueryForExtractTerms(Query query)
        {
            if (query is BooleanQuery bq)
            {
                foreach (BooleanClause clause in bq.Clauses)
                {
                    //recurse
                    var check = CheckQueryForExtractTerms(clause.Query);
                    if (!check)
                    {
                        return false;
                    }
                }
            }

            if (query is LateBoundQuery lbq)
            {
                return CheckQueryForExtractTerms(lbq.Wrapped);
            }

            Type queryType = query.GetType();

            if (typeof(TermRangeQuery).IsAssignableFrom(queryType)
                || typeof(WildcardQuery).IsAssignableFrom(queryType)
                || typeof(FuzzyQuery).IsAssignableFrom(queryType)
                || (queryType.IsGenericType && queryType.GetGenericTypeDefinition().IsAssignableFrom(typeof(NumericRangeQuery<>))))
            {
                return false; //ExtractTerms() not supported by TermRangeQuery, WildcardQuery,FuzzyQuery and will throw NotSupportedException 
            }

            return true;
        }
    }
}
