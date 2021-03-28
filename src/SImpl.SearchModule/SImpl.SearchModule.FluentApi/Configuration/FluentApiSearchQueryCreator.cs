using System;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.FluentApi.Configuration.Fluent;

namespace SImpl.SearchModule.FluentApi.Configuration
{
    public class FluentApiSearchQueryCreator : IFluentApiSearchQueryCreator
    {
        public ISearchQuery CreateSearchQuery(Action<FluentQueryConfigurator> configurator)
        {
            var newQuery = new FluentQueryConfigurator();
            configurator.Invoke(newQuery);
            return (ISearchQuery)newQuery.Query;
        }

        
    }
}