using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.FluentApi.Queries
{
    public class TermSubQuery : ISearchSubQuery
    {
        public Occurance Occurance { get; set; }
        public string Field { get; set; }
        public object Value { get; set; }
    }
}