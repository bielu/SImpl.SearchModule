using System.Collections.Generic;
using Examine;
using Examine.Lucene.Search;
using Examine.Search;
using Lucene.Net.Analysis;
using Lucene.Net.Search;

namespace SImpl.SearchModule.Examine.Application.LuceneEngine
{
    public class LuceneSearchQueryWithFiltersAndFacets : LuceneSearchQuery
    {
        private readonly ISearchContext _searchContext;
        private ISet<string> _fieldsToLoad = null;
        private BooleanQuery FilterQuery;
        private Dictionary<string, Query> highlighterQueries;

        public LuceneSearchQueryWithFiltersAndFacets(ISearchContext searchContext, string category, Analyzer analyzer, LuceneSearchOptions searchOptions, BooleanOperation occurance) : base(searchContext, category, analyzer, searchOptions, occurance)
        {
            _searchContext = searchContext;
        }
        public ISearchResults Execute(QueryOptions options = null) => Search(options);
        private ISearchResults Search(QueryOptions options)
        {
            // capture local
            var query = Query;

            if (!string.IsNullOrEmpty(Category))
            {
                // rebuild the query
                IList<BooleanClause> existingClauses = query.Clauses;

                if (existingClauses.Count == 0)
                {
                    // Nothing to search. This can occur in cases where an analyzer for a field doesn't return
                    // anything since it strips all values.
                    return EmptySearchResults.Instance;
                }

                query = new BooleanQuery
                {
                    // prefix the category field query as a must
                    { GetFieldInternalQuery(ExamineFieldNames.CategoryFieldName, new ExamineValue(Examineness.Explicit, Category), false), Occur.MUST }
                };

                // add the ones that we're already existing
                foreach (var c in existingClauses)
                {
                    query.Add(c);
                }
            }
            Filter filter= new QueryWrapperFilter(FilterQuery);

            var executor = new LuceneSearchExecutorWithFacetsAndFilters(options, query, filter,highlighterQueries, SortFields, _searchContext, _fieldsToLoad);

            var pagesResults = executor.Execute();

            return pagesResults;
        }

        public void Not(Query translate)
        {
            Query.Add(translate, Occur.MUST_NOT);
        }

        public void And(Query translate)
        {
            Query.Add(translate, Occur.MUST);
        }
        public void Or(Query translate)
        {
            Query.Add(translate, Occur.SHOULD);
        }
    }
}