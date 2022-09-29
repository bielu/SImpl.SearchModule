using System.Collections.Generic;

namespace SImpl.SearchModule.Abstraction.Queries
{
    public class TermAggregation : IAggregationQuery
    {
        public string AggregationName { get; set; }
        public string TermFieldName { get; set; }
        public List<IAggregationQuery> NestedAggregations { get; set; } = new List<IAggregationQuery>();
        public int Size { get; set; } = 1000;
    }
}