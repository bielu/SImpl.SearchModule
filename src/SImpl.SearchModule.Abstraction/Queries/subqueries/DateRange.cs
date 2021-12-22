using System;

namespace SImpl.SearchModule.Abstraction.Queries.subqueries
{
    public class DateRange : BaseRangeQuery
    {
        public DateTime? MinValue { get; set; }
        public DateTime? MaxValue { get; set; }
    }
}