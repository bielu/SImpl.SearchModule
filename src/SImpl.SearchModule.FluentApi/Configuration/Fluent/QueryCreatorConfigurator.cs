using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public class QueryCreatorConfigurator
    {
        public ISearchQuery<IQueryResult> Query { get; set; }
        public QueryCreatorConfigurator(SearchQuery searchQuery)
        {
          
        }

        public QueryCreatorConfigurator WithPageSize(int pageSize)
        {
            Query.PageSize = pageSize;
            return this;
        }
        public QueryCreatorConfigurator WithPage(int page)
        {
            Query.Page = page;
            return this;
        }
        public QueryCreatorConfigurator WithIndex(string IndexName)
        {
            Query.Index = IndexName;
            return this;
        }
    }
}