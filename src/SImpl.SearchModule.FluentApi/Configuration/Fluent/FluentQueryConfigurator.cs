using System;
using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public class FluentQueryConfigurator : IQueryConfigurator
    {
        public FluentQueryConfigurator()
        {
        }

        public INestableQuery Query { get; set; }
      
    }
}