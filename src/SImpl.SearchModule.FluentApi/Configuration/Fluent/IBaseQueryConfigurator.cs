using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public class IBaseQueryConfigurator
    {
        public ISearchQuery<IQueryResult> Query { get; set; }
    }
}