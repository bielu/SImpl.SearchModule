using System;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent.Extensions
{
    public static class CreateQueryHelper
    {
        public static IQueryConfigurator CreateTermQuery(this IQueryConfigurator configurator, Action<TermQueryConfigurator> query){
            var booleanQuery = new TermQueryConfigurator();
            query.Invoke(booleanQuery);
            configurator.Query.Add(booleanQuery.Query.Occurance,booleanQuery.Query);
            return configurator;
        }
        public static IQueryConfigurator CreateBoolQuery(this IQueryConfigurator configurator, Action<BooleanQueryConfigurator> query){
            var booleanQuery = new BooleanQueryConfigurator();
            query.Invoke(booleanQuery);
            configurator.Query.Add(booleanQuery.Query.Occurance,booleanQuery.Query);
            return configurator;
        }
        public static IBaseQueryConfigurator CreateTermQuery(this IBaseQueryConfigurator configurator, Action<TermQueryConfigurator> query){
            var booleanQuery = new TermQueryConfigurator();
            query.Invoke(booleanQuery);
            configurator.Query.Add(booleanQuery.Query.Occurance,booleanQuery.Query);
            return configurator;
        }
        public static IBaseQueryConfigurator CreateBoolQuery(this IBaseQueryConfigurator configurator, Action<BooleanQueryConfigurator> query){
            var booleanQuery = new BooleanQueryConfigurator();
            query.Invoke(booleanQuery);
            configurator.Query.Add(booleanQuery.Query.Occurance,booleanQuery.Query);
            return configurator;
        }
    }
}