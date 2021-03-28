using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;

namespace SImpl.SearchModule.Abstraction.Handlers
{
    public interface ISearchQueryHandler<in TQuery> : ISearchQueryHandler<TQuery, IQueryResult>
        where TQuery : class, ISearchQuery
    {
    }
    public interface ISearchQueryHandler<in TQuery, out TOutput>
        where TQuery : class, ISearchQuery
        where TOutput : IQueryResult
    {
        
    }
}