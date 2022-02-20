using System.Collections.Generic;
using System.Linq;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Results;

namespace SImpl.SearchModule.Core.Helpers
{
    public static class TranslationHelperExtensions
    {
        public static BucketAggregation RetrieveBucketAggregation(this SimplQueryResult queryResult,
            string aggregationName)
        {
            return queryResult.Aggregations.Select(x => x as BucketAggregation)
                .Where(x => x != null)
                .FirstOrDefault(x => x.AggregationName == aggregationName);
        }
        public static  SingleAggregation RetrieveSingleAggregation(this SimplQueryResult queryResult,
            string aggregationName)
        {
            return queryResult.Aggregations.Select(x => x as SingleAggregation)
                .Where(x => x != null)
                .FirstOrDefault(x => x.AggregationName == aggregationName);
        }
    }
}