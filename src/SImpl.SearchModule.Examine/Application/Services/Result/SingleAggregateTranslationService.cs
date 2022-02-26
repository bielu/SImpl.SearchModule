using System.Collections.Generic;
using SImpl.SearchModule.Abstraction.Models;
using IAggregation = SImpl.SearchModule.Abstraction.Models.IAggregation;

namespace SImpl.SearchModule.Examine.Application.Services.Result
{
    public class SingleAggregateTranslationService : IAggregationTranslationService<SingleBucketAggregate>
    {
        public IAggregation Translate(KeyValuePair<string, IAggregate> aggregation)
        {
            var aggregate = aggregation.Value as SingleBucketAggregate;
            return new SingleAggregation()
            {
                AggregationName = aggregation.Key,
             };
        }

        
    }
}