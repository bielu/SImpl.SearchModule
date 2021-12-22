namespace SImpl.SearchModule.Abstraction.Queries.subqueries
{
    public class StringRange : BaseRangeQuery
    {
        public string? MinValue { get; set; }
        public string? MaxValue { get; set; }
    }
}