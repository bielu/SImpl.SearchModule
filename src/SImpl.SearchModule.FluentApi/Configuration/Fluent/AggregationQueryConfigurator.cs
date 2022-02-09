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

        public AggregationQueryConfigurator CreateFilterQuery(Action<FilterFacetQueryConfigurator> query)
        {
            var configurator = new FilterFacetQueryConfigurator(new FilterFacetQuery());
            query.Invoke(configurator);
            _configuratorQuery.FacetQueries.Add(configurator.FilterFacetQuery);
            return this;
        }

        
    }
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
    public class FilterFacetQueryConfigurator : IAggregationConfigurator
    {
        public FilterFacetQuery FilterFacetQuery { get; }

        public FilterFacetQueryConfigurator(FilterFacetQuery filterFacetQuery)
        {
            FilterFacetQuery = filterFacetQuery;
            Query = new BoolSearchSubQuery();
            FilterFacetQuery.Queries.Add(Occurance.Must,Query);
        }


        public INestableQuery Query { get; set; }
        public Occurance Occurance { get; set; }
    }
}