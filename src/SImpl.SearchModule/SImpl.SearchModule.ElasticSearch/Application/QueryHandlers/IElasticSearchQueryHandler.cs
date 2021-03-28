using System.Threading.Tasks;
using SImpl.SearchModule.Abstraction.Handlers;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;

namespace SImpl.SearchModule.ElasticSearch.Application.QueryHandlers
{
    public interface IElasticSearchQueryHandler : ISearchQueryHandler<ISearchQuery<IQueryResult>>
    {
    }
}