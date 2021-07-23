using SImpl.CQRS.Queries;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;

namespace SImpl.SearchModule.Abstraction.Handlers
{
    public interface ISearchQueryHandler<in TQuery> : IQueryHandler<TQuery,IQueryResult>
        where TQuery : class, IQuery<IQueryResult>
    {
    }
    public interface ISearchQueryHandler<in TQuery, TOutput> : IQueryHandler<TQuery,TOutput>
        where TQuery : class, ISearchQuery<TQuery>, IQuery<TOutput>
        where TOutput : IQueryResult
    {
        
    }
}