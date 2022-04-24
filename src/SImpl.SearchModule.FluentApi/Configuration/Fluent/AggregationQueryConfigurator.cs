using System;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;
using SImpl.SearchModule.Abstraction.Results;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public class AggregationQueryConfigurator
    {
        private readonly ISearchQuery<IQueryResult> _configuratorQuery;

        public AggregationQueryConfigurator(ISearchQuery<IQueryResult> configuratorQuery)
        {
            _configuratorQuery = configuratorQuery;
        }

        public AggregationQueryConfigurator CreateFilterQuery(Action<FilterAggregationQueryConfigurator> query)
        {
            var configurator = new FilterAggregationQueryConfigurator(new FilterAggregationQuery());
            query.Invoke(configurator);
            _configuratorQuery.FacetQueries.Add(configurator.FilterAggregationQuery);
            return this;
        }
        public AggregationQueryConfigurator CreateTermQuery(Action<TermAggregationQueryConfigurator> query)
        {
            var configurator = new TermAggregationQueryConfigurator();
            query.Invoke(configurator);
            _configuratorQuery.FacetQueries.Add(configurator.Query);
            return this;
        }
        public AggregationQueryConfigurator CreateMultiTermQuery(Action<TermAggregationQueryConfigurator> query)
        {
            var configurator = new TermAggregationQueryConfigurator();
            query.Invoke(configurator);
            _configuratorQuery.FacetQueries.Add(configurator.Query);
            return this;
        }
        
    }
}