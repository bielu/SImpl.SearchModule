using System;
using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent.Extensions
{
    public static class CreateAggregationQueryHelper
    {
        //todo: better dsl coverage
        public static IBaseQueryConfigurator CreateAggregationQuery<T>(this IBaseQueryConfigurator configurator,
            Action<T> query) where T :  IAggregationQuery, new()
        {
            var booleanQuery = new T();
            query.Invoke(booleanQuery);
            configurator.Query.FacetQueries.Add(booleanQuery);
            return configurator;
        }
        public static IBaseQueryConfigurator CreateAggregationQuery(this IBaseQueryConfigurator configurator,
            Action<AggregationQueryConfigurator> query) 
        {
            var booleanQuery = new AggregationQueryConfigurator( configurator.Query);
            query.Invoke(booleanQuery);
           
            return configurator;
        }
        public static IAggregationConfigurator CreateSubAggregationQuery<T>(this IAggregationConfigurator configurator,
            Action<T> query) where T :  IAggregationQuery, new()
        {
            var booleanQuery = new T();
            query.Invoke(booleanQuery);
            configurator.FilterFacetQuery.NestedAggregations.Add(booleanQuery);
            return configurator;
        }
        public static IAggregationConfigurator CreateSubAggregationQuery(this IAggregationConfigurator configurator,
            Action<AggregationSubQueryConfigurator> query) 
        {
            var booleanQuery = new AggregationSubQueryConfigurator(configurator.FilterFacetQuery);
            query.Invoke(booleanQuery);
           
            return configurator;
        }
    }
}