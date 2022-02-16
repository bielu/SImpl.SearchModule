using System.Collections.Generic;

namespace SImpl.SearchModule.Abstraction.Queries.HighlightQueries
{
    public interface IHighlightQuery
    {
        string Field { get; set; }
        bool IsAllField { get; set; }
        Dictionary<Occurance, ISearchSubQuery> Queries { get; set; }
    }
}