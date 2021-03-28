using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;

namespace SImpl.SearchModule.Client.Application.Services
{
    public interface IContentSearchService
    {
        IReadonlySearchMetaModelResultCollection Search<TSearchQuery>(
            TSearchQuery query)
            where TSearchQuery : class, ISearchQuery;

        IReadonlySearchMetaModelResultCollection<TSearchOutput> Search<TSearchQuery, TSearchOutput>(
            TSearchQuery query)
            where TSearchQuery : class, ISearchQuery
            where TSearchOutput : IQueryResult;
    }
}