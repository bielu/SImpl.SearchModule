using System.Collections.Generic;
using Nest;
using IAggregation = SImpl.SearchModule.Abstraction.Models.IAggregation;

namespace SImpl.SearchModule.ElasticSearch.Application.Services.Result
{
    public interface  IAggregationTranslationService<T> :  IAggregationTranslationService
    {
        
    }
    public interface  IAggregationTranslationService
    {
        IAggregation Translate(KeyValuePair<string, IAggregate> aggregation);
    }
}