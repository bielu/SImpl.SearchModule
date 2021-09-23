using System;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;
using SImpl.SearchModule.Abstraction.Results;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public class FluentQueryConfigurator : IBaseQueryConfigurator
    {
        public FluentQueryConfigurator(ISearchQuery<IQueryResult> baseQuery)
        {
            Query = baseQuery;
        }

        public ISearchQuery<IQueryResult> Query { get; set; }
      
    }
}