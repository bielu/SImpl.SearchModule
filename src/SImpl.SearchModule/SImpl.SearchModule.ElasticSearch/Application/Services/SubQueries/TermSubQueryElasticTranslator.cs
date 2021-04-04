using Nest;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.ElasticSearch.Application.ElasticQueries;

namespace SImpl.SearchModule.ElasticSearch.Application.Commands
{
    public class TermSubQueryElasticTranslator : ISubQueryElasticTranslator<TermSubQuery>
    {
        public QueryContainer Translate<TViewModel>(ISearchSubQuery query) where TViewModel : class
        {
            var castedQuery = (TermSubQuery)query;
            var queryResult = new QueryContainerDescriptor<TViewModel>();
            queryResult.Term(x => x.Field(new Field(castedQuery.Field)).Value(castedQuery.Value));
            return queryResult;
        }
    }
}