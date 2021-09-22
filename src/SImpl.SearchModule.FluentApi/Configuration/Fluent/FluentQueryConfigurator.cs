using System;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public class FluentQueryConfigurator : IQueryConfigurator
    {
        public FluentQueryConfigurator()
        {
            Query = new BoolSearchSubQuery();
        }

        public INestableQuery Query { get; set; }
      
    }
}