namespace SImpl.SearchModule.Abstraction.Queries.subqueries
{
    public class NumericRange : BaseRangeQuery
    {
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
    }
}