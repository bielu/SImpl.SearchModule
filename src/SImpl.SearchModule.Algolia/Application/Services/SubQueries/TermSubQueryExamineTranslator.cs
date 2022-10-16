using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;
using SImpl.SearchModule.Algolia.Application.LuceneEngine;

namespace SImpl.SearchModule.Algolia.Application.Services.SubQueries
{
    public class TermSubQueryExamineTranslator : ISubQueryExamineTranslator<TermSubQuery>
    {
        public Query Translate<TViewModel>(ISearcher searcher,IEnumerable<ISubQueryElasticTranslator> collection, ISearchSubQuery query) where TViewModel : class
        {
            var searcherBase = searcher as BaseLuceneSearcher;
            var nestedQuery = new LuceneSearchQueryWithFiltersAndFacets(searcherBase.GetSearchContext(),"baseSearch" ,searcherBase.LuceneAnalyzer, new LuceneSearchOptions(), MapOccuranceToExamine(query.Occurance));
            var termSubQuery = (TermSubQuery)query;
            if (termSubQuery.Value == null || termSubQuery.Field == null)
            {
                return null;
            }
            nestedQuery.Field(termSubQuery.Field,termSubQuery.Value.ToString());
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