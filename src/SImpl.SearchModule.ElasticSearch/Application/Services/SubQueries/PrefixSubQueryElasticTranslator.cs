using System.Collections.Generic;
using Nest;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;

namespace SImpl.SearchModule.ElasticSearch.Application.Services.SubQueries
{
    public class PrefixSubQueryElasticTranslator: ISubQueryElasticTranslator<PrefixSubQuery>
    {
        public QueryContainerDescriptor<TViewModel> Translate<TViewModel>(IEnumerable<ISubQueryElasticTranslator> collection, ISearchSubQuery query) where TViewModel : class
        {
            var castedQuery = (PrefixSubQuery)query;
            var queryResult = new QueryContainerDescriptor<TViewModel>();
            queryResult.Prefix(x => x.Field(new Field(castedQuery.Field)).Value(castedQuery.Value));
            return queryResult;
        }

      
    
    }
}