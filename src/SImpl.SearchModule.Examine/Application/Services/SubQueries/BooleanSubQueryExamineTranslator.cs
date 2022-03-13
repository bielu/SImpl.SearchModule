using System.Collections.Generic;
using System.Linq;
using Examine;
using Examine.Lucene.Providers;
using Examine.Lucene.Search;
using Examine.Search;
using Lucene.Net.Search;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;
using SImpl.SearchModule.Examine.Application.LuceneEngine;

namespace SImpl.SearchModule.Examine.Application.Services.SubQueries
{
    public class BooleanSubQueryExamineTranslator : ISubQueryExamineTranslator<BoolSearchSubQuery>
    {
      
        public Query Translate<TViewModel>(ISearcher searcher,IEnumerable<ISubQueryElasticTranslator> collection, ISearchSubQuery query) where TViewModel : class
        {
            var searcherBase = searcher as BaseLuceneSearcher;
            var nestedQuery = new LuceneSearchQueryWithFiltersAndFacets(searcherBase.GetSearchContext(),"baseSearch" ,searcherBase.LuceneAnalyzer, new LuceneSearchOptions(), MapOccuranceToExamine(query.Occurance));
            var boolQuery = (BoolSearchSubQuery)query;
            if (!boolQuery.NestedQueries.Any())
            {
                return null;
            }
                var nestedExamineQuery=nestedQuery as LuceneSearchQueryWithFiltersAndFacets;
                foreach (var booleanQuery in boolQuery.NestedQueries)
                {
                    var type = booleanQuery.GetType();
                    var handlerType =
                        typeof(ISubQueryExamineTranslator<>).MakeGenericType(type);

                    var translator =
                        collection.FirstOrDefault(x => x.GetType().GetInterfaces().Any(x => x == handlerType));
                    if (translator == null)
                    {
                        continue;
                    }
                    switch (booleanQuery.Occurance)
                    {
                        case Occurance.MustNot:
                            nestedExamineQuery.Not(translator.Translate<TViewModel>(searcher,collection, booleanQuery));
                            break;
                        case Occurance.Must:
                            nestedExamineQuery.And(translator.Translate<TViewModel>(searcher,collection, booleanQuery));
                            break;
                        case Occurance.Should:
                            nestedExamineQuery.Or(translator.Translate<TViewModel>(searcher,collection, booleanQuery));
                            break;
                    }
                }

                if (!nestedQuery.Query.Clauses.Any())
                {
                    return null;
                }
                return nestedQuery.Query;

        }

        private BooleanOperation MapOccuranceToExamine(Occurance queryOccurance)
        {
            switch (queryOccurance)
            {
                case Occurance.Should:
                    return BooleanOperation.Or;
                case Occurance.Must:
                    return BooleanOperation.And;
                case Occurance.MustNot:
                    return BooleanOperation.Not;
            }
            return BooleanOperation.Or;
        }
    }
}