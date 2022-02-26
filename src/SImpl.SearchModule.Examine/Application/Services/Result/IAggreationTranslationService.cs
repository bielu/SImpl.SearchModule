using System.Collections.Generic;
using IAggregation = SImpl.SearchModule.Abstraction.Models.IAggregation;

namespace SImpl.SearchModule.Examine.Application.Services.Result
{
    public interface  IAggregationTranslationService<T> :  IAggregationTranslationService
    {
        
    }
    public interface  IAggregationTranslationService
    {
        IAggregation Translate(KeyValuePair<string, IAggregate> aggregation);
    }
}