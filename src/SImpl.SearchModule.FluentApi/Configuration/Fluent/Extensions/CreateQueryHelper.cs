using System;
using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent.Extensions
{
    public static class CreateQueryHelper
    {
       
        public static IQueryConfigurator CreateTermQuery(this IQueryConfigurator configurator,
            Action<TermQueryConfigurator> query)
        {
            var booleanQuery = new TermQueryConfigurator();
            query.Invoke(booleanQuery);
            booleanQuery.Query.Occurance = configurator.Occurance;
            configurator.Query.Add(configurator.Occurance, booleanQuery.Query);
            return configurator;
        }
        public static IQueryConfigurator CreateTermsQuery(this IQueryConfigurator configurator,
            Action<TermsQueryConfigurator> query)
        {
            var booleanQuery = new TermsQueryConfigurator();
            query.Invoke(booleanQuery);
            booleanQuery.Query.Occurance = configurator.Occurance;
            configurator.Query.Add(configurator.Occurance, booleanQuery.Query);
            return configurator;
        }
       
        public static IQueryConfigurator CreateFuzzyQuery(this IQueryConfigurator configurator, Action<FuzzyQueryConfigurator> query)
        {
            var booleanQuery = new FuzzyQueryConfigurator();
            query.Invoke(booleanQuery);
            configurator.Query.Add(configurator.Occurance, booleanQuery.Query);
            return configurator;
        }
        public static IQueryConfigurator CreatePrefixQuery(this IQueryConfigurator configurator, Action<PrefixQueryConfigurator> query)
        {
            var booleanQuery = new PrefixQueryConfigurator();
            query.Invoke(booleanQuery);
            configurator.Query.Add(configurator.Occurance, booleanQuery.Query);
            return configurator;
        }
        public static IQueryConfigurator CreateNumericRangeQuery(this IQueryConfigurator configurator,
            Action<NumericRangeQueryConfigurator> query)
        {
            var booleanQuery = new NumericRangeQueryConfigurator();
            query.Invoke(booleanQuery);
            configurator.Query.Add(configurator.Occurance, booleanQuery.Query);
            return configurator;
        }
        public static IQueryConfigurator CreateLongRangeQuery(this IQueryConfigurator configurator,
            Action<LongRangeQueryConfigurator> query)
        {
            var booleanQuery = new LongRangeQueryConfigurator();
            query.Invoke(booleanQuery);
            configurator.Query.Add(configurator.Occurance, booleanQuery.Query);
            return configurator;
        }  public static IQueryConfigurator CreateStringRangeQuery(this IQueryConfigurator configurator,
            Action<StringRangeQueryConfigurator> query)
        {
            var booleanQuery = new StringRangeQueryConfigurator();
            query.Invoke(booleanQuery);
            configurator.Query.Add(configurator.Occurance, booleanQuery.Query);
            return configurator;
        }  public static IQueryConfigurator CreateDateRangeQuery(this IQueryConfigurator configurator,
            Action<DateRangeQueryConfigurator> query)
        {
            var booleanQuery = new DateRangeQueryConfigurator();
            query.Invoke(booleanQuery);
            configurator.Query.Add(configurator.Occurance, booleanQuery.Query);
            return configurator;
        }
        public static IQueryConfigurator CreateBoolQuery(this IQueryConfigurator configurator,
            Action<BooleanQueryConfigurator> query)
        {
            var booleanQuery = new BooleanQueryConfigurator();
            query.Invoke(booleanQuery);
            configurator.Query.Add(configurator.Occurance, booleanQuery.Query);
            return configurator;
        }

        public static IBaseQueryConfigurator CreateTermQuery(this IBaseQueryConfigurator configurator,
            Action<TermQueryConfigurator> query)
        {
            var booleanQuery = new TermQueryConfigurator();
            query.Invoke(booleanQuery);
            booleanQuery.Occurance = configurator.Occurance;
            configurator.Query.Add(configurator.Occurance, booleanQuery.Query);
            return configurator;
        }
        public static IBaseQueryConfigurator CreateTermsQuery(this IBaseQueryConfigurator configurator,
            Action<TermsQueryConfigurator> query)
        {
            var booleanQuery = new TermsQueryConfigurator();
            query.Invoke(booleanQuery);
            booleanQuery.Occurance = configurator.Occurance;
            configurator.Query.Add(configurator.Occurance, booleanQuery.Query);
            return configurator;
        }
        public static IBaseQueryConfigurator CreateBoolQuery(this IBaseQueryConfigurator configurator,
            Action<BooleanQueryConfigurator> query)
        {
            var booleanQuery = new BooleanQueryConfigurator();
            booleanQuery.Occurance = configurator.Occurance;
            query.Invoke(booleanQuery);
            configurator.Query.Add(configurator.Occurance, booleanQuery.Query);
            return configurator;
        }
        public static IBaseQueryConfigurator CreatePostFilterQuery(this IBaseQueryConfigurator configurator,
            Action<BooleanQueryConfigurator> query)
        {
            var booleanQuery = new BooleanQueryConfigurator();
            booleanQuery.Occurance = configurator.Occurance;
            query.Invoke(booleanQuery);
            configurator.Query.PostFilterQuery.Add(configurator.Occurance, booleanQuery.Query);
            return configurator;
        }
    }
}