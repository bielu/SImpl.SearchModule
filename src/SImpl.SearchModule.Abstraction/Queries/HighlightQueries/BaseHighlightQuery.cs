using System.Collections.Generic;

namespace SImpl.SearchModule.Abstraction.Queries.HighlightQueries
{
    public class BaseHighlightQuery : IHighlightQuery
    {
        public string Field { get; set; } 
        public bool IsAllField { get; set; }

        public Dictionary<Occurance, ISearchSubQuery> Queries { get; set; } =
            new Dictionary<Occurance, ISearchSubQuery>();
    }
}