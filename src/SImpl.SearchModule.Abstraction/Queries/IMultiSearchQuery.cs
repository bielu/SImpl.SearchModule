using System;
using System.Collections.Generic;
using SImpl.CQRS.Queries;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Results;

namespace SImpl.SearchModule.Abstraction.Queries
{
    public interface IMultiSearchQuery :  IQuery<IMultiSearchQueryResult>
    {
        public IDictionary<string, ISearchQuery<IQueryResult>> Queries { get; set; }
    }
}