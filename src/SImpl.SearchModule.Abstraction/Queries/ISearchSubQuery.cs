using System.Collections.Generic;

namespace SImpl.SearchModule.Abstraction.Queries
{
    public interface ISearchSubQuery
    {
        Occurance Occurance { get; set; } 
        int BoostValue { get; set; }

    }
}