using System.Collections.Generic;
using Examine;
using Examine.Lucene.Providers;
using Examine.Lucene.Search;
using Examine.Search;
using Lucene.Net.Search;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;
using SImpl.SearchModule.Examine.Application.LuceneEngine;
using FuzzyQuery = SImpl.SearchModule.Abstraction.Queries.FuzzyQuery;

namespace SImpl.SearchModule.Examine.Application.Services.SubQueries
{
    public class FuzzySubQueryElasticTranslator : ISubQueryElasticTranslator<FuzzyQuery>
    {
        public Query Translate<TViewModel>(ISearcher searcher,IEnumerable<ISubQueryElasticTranslator> collection, ISearchSubQuery query) where TViewModel : class
        {
            var searcherBase = searcher as BaseLuceneSearcher;
            var nestedQuery = new LuceneSearchQueryWithFiltersAndFacets(searcherBase.GetSearchContext(),"baseSearch" ,searcherBase.LuceneAnalyzer, new LuceneSearchOptions(), MapOccuranceToExamine(query.Occurance));
            var termSubQuery = (TermSubQuery)query;
            nestedQuery.Field(termSubQuery.Field,termSubQuery.Value.ToString().Fuzzy());
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