using System.Collections.Generic;

namespace SImpl.SearchModule.Abstraction.Queries
{
    public interface INestableQuery : ISearchSubQuery,ICreatableSearchQuery<Occurance, ISearchSubQuery> 
    {
        
        List<ISearchSubQuery> NestedQueries { get; set; }
    }
}