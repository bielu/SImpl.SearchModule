using System.Collections.Generic;
using SImpl.SearchModule.Abstraction.Models;

namespace SImpl.SearchModule.Abstraction.Results
{
    public class SimplQueryResult : IQueryResult
    {
        public List<ISearchModel> SearchModels { get; set; }
        public long Total { get; set; }
        public int Page { get; set; }
    }
}