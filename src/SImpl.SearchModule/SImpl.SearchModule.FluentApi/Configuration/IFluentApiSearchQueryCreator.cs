using System;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.FluentApi.Configuration.Fluent;

namespace SImpl.SearchModule.FluentApi.Configuration
{
    public interface IFluentApiSearchQueryCreator
    {
        ISearchQuery CreateSearchQuery(Action<FluentQueryConfigurator> configurator);
    }
}