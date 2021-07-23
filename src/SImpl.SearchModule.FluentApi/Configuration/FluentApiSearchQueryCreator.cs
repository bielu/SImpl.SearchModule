using System;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;
using SImpl.SearchModule.FluentApi.Configuration.Fluent;

namespace SImpl.SearchModule.FluentApi.Configuration
{
    public class FluentApiSearchQueryCreator : IFluentApiSearchQueryCreator
    {
        public ISearchQuery<IQueryResult> CreateSearchQuery(Action<FluentQueryConfigurator> configurator)
        {
            var newQuery = new FluentQueryConfigurator();
            configurator.Invoke(newQuery);
            return (ISearchQuery<IQueryResult>)newQuery.Query;
        }

        
    }
}