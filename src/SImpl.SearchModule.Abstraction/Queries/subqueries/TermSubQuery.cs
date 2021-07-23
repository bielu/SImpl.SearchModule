namespace SImpl.SearchModule.Abstraction.Queries.subqueries
{
    public class TermSubQuery : ISearchSubQuery
    {
        public Occurance Occurance { get; set; }
        public string Field { get; set; }
        public object Value { get; set; }
    }
}