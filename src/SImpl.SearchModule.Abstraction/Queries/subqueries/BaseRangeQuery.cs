using SImpl.SearchModule.Abstraction.Enums;

namespace SImpl.SearchModule.Abstraction.Queries.subqueries
{
    public class BaseRangeQuery : ISearchSubQuery
    {
        public Occurance Occurance { get; set; }
        public int BoostValue { get; set; } = 1;
        public string Field { get; set; }
        public bool IncludeMinEdge { get; set; }
        public bool IncludeMaxEdge { get; set; }
    }
}