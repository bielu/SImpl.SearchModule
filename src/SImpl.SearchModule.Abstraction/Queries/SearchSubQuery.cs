using System.Collections.Generic;

namespace SImpl.SearchModule.Abstraction.Queries
{
    public class SearchSubQuery : ISearchSubQuery
    {
        public Occurance Occurance { get; set; } = Occurance.Must;
        public string Field { get; set; }
        public int BoostValue { get; set; } = 1;
        public List<ISearchSubQuery> NestedQueries { get; set; }
    }
}