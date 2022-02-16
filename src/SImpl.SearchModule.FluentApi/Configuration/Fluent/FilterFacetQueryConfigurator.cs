using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
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