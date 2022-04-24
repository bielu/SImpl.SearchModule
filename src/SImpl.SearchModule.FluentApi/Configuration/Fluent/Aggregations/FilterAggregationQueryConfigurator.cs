using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public class FilterAggregationQueryConfigurator : IAggregationConfigurator
    {
        public FilterAggregationQuery FilterAggregationQuery { get; }

        public FilterAggregationQueryConfigurator(FilterAggregationQuery filterAggregationQuery)
        {
            FilterAggregationQuery = filterAggregationQuery;
            Query = new BoolSearchSubQuery();
            FilterAggregationQuery.Queries.Add(Occurance.Must,Query);
        }


        public INestableQuery Query { get; set; }
        public Occurance Occurance { get; set; }
    }
}