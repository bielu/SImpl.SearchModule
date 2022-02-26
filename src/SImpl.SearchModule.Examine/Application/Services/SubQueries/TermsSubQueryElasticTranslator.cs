using System.Collections.Generic;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;

namespace SImpl.SearchModule.Examine.Application.Services.SubQueries
{
    public class TermsSubQueryElasticTranslator : ISubQueryElasticTranslator<TermsSubQuery>
    {
        public QueryContainerDescriptor<TViewModel> Translate<TViewModel>(IEnumerable<ISubQueryElasticTranslator> collection, ISearchSubQuery query) where TViewModel : class
        {
            var castedQuery = (TermsSubQuery)query;
            var queryResult = new QueryContainerDescriptor<TViewModel>();
            queryResult.Terms(x => x.Field(new Field(castedQuery.Field)).Terms(castedQuery.Value));
            return queryResult;
        }

      
    }
}