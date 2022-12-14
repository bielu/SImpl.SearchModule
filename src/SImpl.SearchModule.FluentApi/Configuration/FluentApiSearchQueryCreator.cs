using System;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;
using SImpl.SearchModule.FluentApi.Configuration.Fluent;

namespace SImpl.SearchModule.FluentApi.Configuration
{
    public class FluentApiMultiSearchQueryCreator : IFluentApiMultiSearchQueryCreator
    {
        private  IMultiSearchQuery _baseQuery { get; set; }

        public FluentApiMultiSearchQueryCreator()
        {
            _baseQuery = new MultiSearchQuery();
        }
        public IMultiSearchQuery Compile()
        {
            return _baseQuery;
        }
        public FluentApiMultiSearchQueryCreator CreateSearchQuery(string key, SearchQuery searchQuery,Action<IBaseQueryConfigurator> configurator )
        {
            var baseQuery = new QueryCreatorConfigurator(searchQuery);
            var query = new FluentApiSearchQueryCreator(baseQuery.Query)
                .CreateSearchQuery(queryConfigurator => configurator.Invoke(new FluentQueryConfigurator(baseQuery.Query)));
            _baseQuery.Queries.Add(key,query );
            return this;
        }
        public FluentApiMultiSearchQueryCreator CreateSearchQuery(string key,Action<QueryCreatorConfigurator> searchQuery,Action<IBaseQueryConfigurator> configurator )
        {
            var baseQuery = new QueryCreatorConfigurator(new SearchQuery());
            searchQuery.Invoke(baseQuery);
            var query = new FluentApiSearchQueryCreator(baseQuery.Query)
                .CreateSearchQuery(queryConfigurator => configurator.Invoke(new FluentQueryConfigurator(baseQuery.Query)));
            _baseQuery.Queries.Add(key,query );
            return this;
        }
    }

    public interface IFluentApiMultiSearchQueryCreator
    {
        FluentApiMultiSearchQueryCreator CreateSearchQuery(string key, Action<QueryCreatorConfigurator> searchQuery,
            Action<IBaseQueryConfigurator> configurator);
    
        IMultiSearchQuery Compile();
    }

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