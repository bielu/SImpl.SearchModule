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
            Occurance = Occurance.Must;
            return this;
        }
        public BooleanQueryConfigurator MustNot()
        {
            Occurance = Occurance.MustNot;
            return this;
        }
        public BooleanQueryConfigurator Should()
        {
            Occurance = Occurance.Should;
            return this;
        }

        public BooleanQueryConfigurator Filter()
        {
            Occurance = Occurance.Filter;
            return this;
        }
        public INestableQuery Query { get; set; }
        public Occurance Occurance { get; set; }
    }
}