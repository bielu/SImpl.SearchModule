using System;
using SImpl.SearchModule.Abstraction.Queries;
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

        public AggregationQueryConfigurator CreateFilterQuery(Action<FilterFacetQueryConfigurator> query)
        {
            var configurator = new FilterFacetQueryConfigurator(new FilterFacetQuery());
            query.Invoke(configurator);
            _configuratorQuery.FacetQueries.Add(configurator.FilterFacetQuery);
            return this;
        }

        
    }
}