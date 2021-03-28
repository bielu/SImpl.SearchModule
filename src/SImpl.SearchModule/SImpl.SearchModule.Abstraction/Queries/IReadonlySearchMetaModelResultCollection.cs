using System.Collections.Generic;

namespace SImpl.SearchModule.Abstraction.Queries
{
    public interface IReadonlySearchMetaModelResultCollection<out TOutput> : 
        IReadonlySearchMetaModelResultCollection
    {
        TOutput GetOutput();
    }
    public interface IReadonlySearchMetaModelResultCollection
    {
        int Total { get; }

        int Page { get; }

        IEnumerable<TViewModel> GetSearchResult<TViewModel>();
    }
}