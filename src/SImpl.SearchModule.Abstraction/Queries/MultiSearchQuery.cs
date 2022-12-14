using System.Collections.Generic;
using SImpl.CQRS.Queries;
using SImpl.SearchModule.Abstraction.Models;

namespace SImpl.SearchModule.Abstraction.Queries
{
    public class MultiSearchQuery : IMultiSearchQuery, IQuery<IMultiSearchQueryResult>
    {
        public IDictionary<string, ISearchQuery<ISearchModel>> Queries { get; set; }
    }
}