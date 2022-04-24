using System;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;
using SImpl.SearchModule.FluentApi.Configuration.Fluent;

namespace SImpl.SearchModule.FluentApi.Configuration
{
    public class FluentApiSearchQueryCreator : IFluentApiSearchQueryCreator
    {
        private  ISearchQuery<IQueryResult> _baseQuery { get; set; }

        public FluentApiSearchQueryCreator(ISearchQuery<IQueryResult> baseQuery)
        {
            _baseQuery = baseQuery;
        }
        public FluentApiSearchQueryCreator()
        {
          
        }

        public FluentApiSearchQueryCreator WithSearchQuery(Action<QueryCreatorConfigurator> configurator)
        {
            var query = new QueryCreatorConfigurator(new SearchQuery());
            configurator.Invoke(query);
          _baseQuery =   query.Query;
          return this;
        }
        public ISearchQuery<IQueryResult> CreateSearchQuery(Action<IBaseQueryConfigurator> configurator)
        {
            var newQuery = new FluentQueryConfigurator(_baseQuery);
            newQuery.Occurance = Occurance.Must;
            configurator.Invoke(newQuery);
            return (ISearchQuery<IQueryResult>)newQuery.Query;
        }
    }
}