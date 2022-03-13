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
    public class TermsSubQueryExamineTranslator : ISubQueryExamineTranslator<TermsSubQuery>
    {
        public Query Translate<TViewModel>(ISearcher searcher, IEnumerable<ISubQueryElasticTranslator> collection,
            ISearchSubQuery query) where TViewModel : class
        {
            var searcherBase = searcher as BaseLuceneSearcher;
            
            var nestedQuery = new LuceneSearchQueryWithFiltersAndFacets(searcherBase.GetSearchContext(), "baseSearch",
                searcherBase.LuceneAnalyzer, new LuceneSearchOptions(), MapOccuranceToExamine(query.Occurance));
            var termSubQuery = (TermsSubQuery)query;
            if (termSubQuery.Value == null || termSubQuery.Field == null)
            {
                return null;
            }
            nestedQuery.GroupedAnd(new List<string>() { termSubQuery.Field }.ToArray(),
                termSubQuery.Value.Select(x => x.ToString()).ToArray());
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