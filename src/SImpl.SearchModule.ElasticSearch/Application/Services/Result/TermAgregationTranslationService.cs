using System.Collections.Generic;
using System.Linq;
using Nest;
using SImpl.SearchModule.Abstraction.Models;
using IAggregation = SImpl.SearchModule.Abstraction.Models.IAggregation;
using IBucket = SImpl.SearchModule.Abstraction.Models.IBucket;

namespace SImpl.SearchModule.ElasticSearch.Application.Services.Result
{
    public class BucketAggregateTranslationService : IAggregationTranslationService<BucketAggregate>
    {
        public IAggregation Translate(KeyValuePair<string, IAggregate> aggregation)
        {
            var aggregate = aggregation.Value as BucketAggregate;
            return new BucketAggregation()
            {
                AggregationName = aggregation.Key,
                Buckets = aggregate.Items.Select(x => TranslateTermBucket<ISearchModel>(x as KeyedBucket<object>) as IBucket).ToList()
            };
        }

        private KeyBucket TranslateTermBucket<TResult>(KeyedBucket<object> keyedBucket)
        {
            if (keyedBucket == null)
            {
                return null;
            }
            var bucket = new KeyBucket()
            {
                Key = keyedBucket.Key.ToString(),
                Keys = keyedBucket.Keys,
                IsComplex = keyedBucket.Keys.Count()>1,
                TotalOfDocuments = keyedBucket.DocCount,
            };
            return bucket;
        }
    }
}