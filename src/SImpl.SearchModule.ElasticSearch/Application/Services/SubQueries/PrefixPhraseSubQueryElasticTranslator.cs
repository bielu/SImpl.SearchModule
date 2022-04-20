using System.Collections.Generic;
using Nest;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;

namespace SImpl.SearchModule.ElasticSearch.Application.Services.SubQueries
{
    public class PrefixPhraseSubQueryElasticTranslator: ISubQueryElasticTranslator<PrefixPhraseSubQuery>
    {
        public QueryContainerDescriptor<TViewModel> Translate<TViewModel>(IEnumerable<ISubQueryElasticTranslator> collection, ISearchSubQuery query) where TViewModel : class
        {
            var castedQuery = (PrefixPhraseSubQuery)query;
            var queryResult = new QueryContainerDescriptor<TViewModel>();
            queryResult.MatchPhrasePrefix(x => x.Field(castedQuery.Field).Query(castedQuery.Value.ToString()).Boost(castedQuery.BoostValue));
            return queryResult;
        }

      
    
    }
}