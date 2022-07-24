using System.Linq;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public class SpatialQueryConfigurator
    {
        public SpatialQueryConfigurator()
        {
            _query = new SpatialSearchQuery();
        }

        public SpatialQueryConfigurator Must()
        {
            Occurance = Occurance.Must;
            return this;
        }
        public SpatialQueryConfigurator MustNot()
        {
            Occurance = Occurance.MustNot;
            return this;
        }
        public SpatialQueryConfigurator Should()
        {
            Occurance = Occurance.Should;
            return this;
        }

        public SpatialQueryConfigurator Filter()
        {
            Occurance = Occurance.Filter;
            return this;
        }

        public SpatialQueryConfigurator WithField(string fieldName)
        {
            _query.Field = fieldName;
            return this;  
        }
        public SpatialQueryConfigurator WithDistance(double value)
        {
            _query.Distance = value;
            return this;
        }
        public SpatialQueryConfigurator WithUnitOfDistance(string value)
        {
            _query.UnitOfDistance = value;
            return this;
        }

        private SpatialSearchQuery _query;
        public ISearchSubQuery Query => _query;
        public Occurance Occurance { get; set; }
    }
}