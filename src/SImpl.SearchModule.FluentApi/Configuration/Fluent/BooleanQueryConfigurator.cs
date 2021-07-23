using System.Collections.Generic;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public class BooleanQueryConfigurator : IQueryConfigurator
    {
        public BooleanQueryConfigurator()
        {
            Query = new BoolSearchSubQuery();
        }

        public BooleanQueryConfigurator Must()
        {
            Query.Occurance = Occurance.Must;
            return this;
        }
        public BooleanQueryConfigurator MustNot()
        {
            Query.Occurance = Occurance.MustNot;
            return this;
        }
        public BooleanQueryConfigurator Should()
        {
            Query.Occurance = Occurance.Should;
            return this;
        }

        public BooleanQueryConfigurator Filter()
        {
            Query.Occurance = Occurance.Filter;
            return this;
        }
        public INestableQuery Query { get; set; }
    }
}