using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Algolia.Application.LuceneEngine;
using LongRange = SImpl.SearchModule.Abstraction.Queries.subqueries.LongRange;

namespace SImpl.SearchModule.Algolia.Application.Services.SubQueries
{
    public class LongRangeQueryExamineTranslator : ISubQueryExamineTranslator<LongRange>
    {
        public Query Translate<TViewModel>(ISearcher searcher, IEnumerable<ISubQueryElasticTranslator> collection,
            ISearchSubQuery query) where TViewModel : class
        {
            var searcherBase = searcher as BaseLuceneSearcher;
            var nestedQuery = new LuceneSearchQueryWithFiltersAndFacets(searcherBase.GetSearchContext(), "baseSearch",
                searcherBase.LuceneAnalyzer, new LuceneSearchOptions(), MapOccuranceToExamine(query.Occurance));
            var termSubQuery = (LongRange)query;
            if (termSubQuery.MinValue == null && termSubQuery.MaxValue == null || termSubQuery.Field == null)
            {
                return null;
            }
            nestedQuery.RangeQuery<long>(
                new[] { termSubQuery.Field },
                termSubQuery.MinValue,
                termSubQuery.MaxValue,
                maxInclusive: termSubQuery.IncludeMaxEdge, minInclusive: termSubQuery.IncludeMinEdge);
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