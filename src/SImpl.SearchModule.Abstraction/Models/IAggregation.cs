using System.Collections.Generic;

namespace SImpl.SearchModule.Abstraction.Models
{
    public interface IAggregation
    {
        public string AggregationName { get; set; }
        IEnumerable<IAggregation> NestedAggregation { get; set; }
    }
}