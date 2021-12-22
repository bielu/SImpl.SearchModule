namespace SImpl.SearchModule.Abstraction.Queries.subqueries
{
    public class LongRange : BaseRangeQuery
    {
        public long? MinValue { get; set; }
        public long? MaxValue { get; set; }
    }
}