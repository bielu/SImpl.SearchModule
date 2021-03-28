using System;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;
using SImpl.SearchModule.FluentApi.Configuration.Fluent;

namespace SImpl.SearchModule.FluentApi.Configuration
{
    public interface IFluentApiSearchQueryCreator
    {
        ISearchQuery<IQueryResult> CreateSearchQuery(Action<FluentQueryConfigurator> configurator);
    }
}