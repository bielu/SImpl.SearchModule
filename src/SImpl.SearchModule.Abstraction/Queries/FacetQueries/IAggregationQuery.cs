using System.Collections.Generic;

namespace SImpl.SearchModule.Abstraction.Queries
{
    public interface IAggregationQuery
    {
        string AggregationName { get; set; }
        public List<IAggregationQuery> NestedAggregations{ get; set; }
    }
}