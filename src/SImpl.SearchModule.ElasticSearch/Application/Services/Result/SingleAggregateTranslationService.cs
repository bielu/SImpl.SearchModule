using System.Collections.Generic;
using System.Linq;
using Nest;
using SImpl.SearchModule.Abstraction.Models;
using IAggregation = SImpl.SearchModule.Abstraction.Models.IAggregation;
using IBucket = SImpl.SearchModule.Abstraction.Models.IBucket;

namespace SImpl.SearchModule.ElasticSearch.Application.Services.Result
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