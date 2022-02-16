using System;
using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public class AggregationSubQueryConfigurator
    {
        private readonly IAggregationQuery _configuratorQuery;

        public AggregationSubQueryConfigurator(IAggregationQuery configuratorQuery)
        {
            _configuratorQuery = configuratorQuery;
        }

        public AggregationSubQueryConfigurator CreateFilterQuery(Action<FilterFacetQueryConfigurator> query)
        {
            var configurator = new FilterFacetQueryConfigurator(new FilterFacetQuery());
            query.Invoke(configurator);
            _configuratorQuery.NestedAggregations.Add(configurator.FilterFacetQuery);
            return this;
        }

        
    }
}