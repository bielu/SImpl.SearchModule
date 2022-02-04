using System.Collections.Generic;
using SImpl.SearchModule.Abstraction.Models;

namespace SImpl.SearchModule.Abstraction.Results
{
    public interface IQueryResult
    {
        List<ISearchModel> SearchModels { get; set; }
        Pagination Pagination { get; set; }
    }
}