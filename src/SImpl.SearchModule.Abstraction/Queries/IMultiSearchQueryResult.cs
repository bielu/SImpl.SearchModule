using System.Collections.Generic;
using SImpl.SearchModule.Abstraction.Results;

namespace SImpl.SearchModule.Abstraction.Queries
{
    public class MultiSearchQueryResult : IMultiSearchQueryResult
    {
        public IDictionary<string, IQueryResult> Results { get; set; }
    }
    public interface IMultiSearchQueryResult
    {
        public IDictionary<string, IQueryResult> Results { get; set; }
    }
}