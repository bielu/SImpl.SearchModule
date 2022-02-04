using System.Collections.Generic;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public interface IBaseQueryConfigurator
    {
        public ISearchQuery<IQueryResult> Query { get; set; }
      

        Occurance Occurance { get; set; }
    }
}