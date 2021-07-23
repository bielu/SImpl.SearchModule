using System.Collections.Generic;

namespace SImpl.SearchModule.Abstraction.Queries
{
    public class SearchSubQuery : ISearchSubQuery
    {
        public Occurance Occurance { get; set; } = Occurance.Must;
        public List<ISearchSubQuery> NestedQueries { get; set; }
    }
}