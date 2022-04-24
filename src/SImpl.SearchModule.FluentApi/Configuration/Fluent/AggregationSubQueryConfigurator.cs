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

        public AggregationSubQueryConfigurator CreateFilterQuery(Action<FilterAggregationQueryConfigurator> query)
        {
            var configurator = new FilterAggregationQueryConfigurator(new FilterAggregationQuery());
            query.Invoke(configurator);
            _configuratorQuery.NestedAggregations.Add(configurator.FilterAggregationQuery);
            return this;
        }

        
    }
}