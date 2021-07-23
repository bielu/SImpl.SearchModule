using System.Collections.Generic;
using SImpl.SearchModule.Abstraction.Models;

namespace SImpl.SearchModule.Abstraction.Results
{
    public interface IQueryResult
    {
        List<ISearchModel> SearchModels { get; set; }
        long Total { get; set; }
        int Page { get; set; }
    }
}