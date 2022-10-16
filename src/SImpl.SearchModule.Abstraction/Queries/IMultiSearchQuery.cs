using System;
using System.Collections.Generic;
using SImpl.CQRS.Queries;

namespace SImpl.SearchModule.Abstraction.Queries
{
    public interface IMultiSearchQuery :  IQuery<IMultiSearchQueryResult>
    {
        public IDictionary<string, ISearchQuery> Queries { get; set; }
    }
}