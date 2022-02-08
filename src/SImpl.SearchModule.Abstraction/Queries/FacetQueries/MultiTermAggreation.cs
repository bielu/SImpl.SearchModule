using System.Collections.Generic;

namespace SImpl.SearchModule.Abstraction.Queries
{
    public class MultiTermAggreation : IAggregationQuery
    {
        public string AggregationName { get; set; }
        public List<SimpleTerm> Terms { get; set; }
        public List<IAggregationQuery> NestedAggregations { get; set; } = new List<IAggregationQuery>();

       
    }
    //todo: implement missing fields
    public class SimpleTerm
    {
        public string TermFieldName { get; set; }
    }
}