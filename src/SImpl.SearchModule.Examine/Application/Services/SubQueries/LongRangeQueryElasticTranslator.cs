using System;
using System.Collections.Generic;
using SImpl.SearchModule.Abstraction.Queries;
using LongRange = SImpl.SearchModule.Abstraction.Queries.subqueries.LongRange;

namespace SImpl.SearchModule.Examine.Application.Services.SubQueries
{
    public class LongRangeQueryElasticTranslator : ISubQueryElasticTranslator<LongRange>
    {
        public QueryContainerDescriptor<TViewModel> Translate<TViewModel>(
            IEnumerable<ISubQueryElasticTranslator> collection, ISearchSubQuery query) where TViewModel : class
        {
            var castedQuery = (LongRange)query;
            var queryResult = new QueryContainerDescriptor<TViewModel>();
      
            queryResult.LongRange(x =>
            {
                var range = new LongRangeQueryDescriptor<TViewModel>();
                if (castedQuery.IncludeMaxEdge && castedQuery.MaxValue.HasValue && (!castedQuery.MinValue.HasValue || castedQuery.MaxValue > castedQuery.MinValue))
                {
                    range=range.LessThanOrEquals(castedQuery.MaxValue.Value);
                }
                else if(castedQuery.MaxValue.HasValue && (!castedQuery.MinValue.HasValue || castedQuery.MaxValue > castedQuery.MinValue))
                {
                    range=range.LessThan(castedQuery.MaxValue.Value);
                }
                else if(castedQuery.MaxValue.HasValue)
                {
                    throw new ArgumentException("Max value has to be higher than minimal value");
                }
                if (castedQuery.IncludeMinEdge && castedQuery.MinValue.HasValue && (!castedQuery.MaxValue.HasValue || castedQuery.MaxValue > castedQuery.MinValue))
                {
                    range=range.LessThanOrEquals(castedQuery.MaxValue.Value);
                }
                else if( castedQuery.MinValue.HasValue &&  (!castedQuery.MaxValue.HasValue || castedQuery.MaxValue < castedQuery.MinValue))
                {
                    range=range.LessThan(castedQuery.MaxValue.Value);
                }
                else if(castedQuery.MaxValue.HasValue)
                {
                    throw new ArgumentException("min value has to be lower than Max value");
                }
                return range;
            });
 
            
            return queryResult;
        }
    }
}