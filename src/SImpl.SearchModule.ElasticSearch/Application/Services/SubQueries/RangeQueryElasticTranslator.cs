using System;
using System.Collections.Generic;
using Nest;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;
using RangeType = SImpl.SearchModule.Abstraction.Enums.RangeType;

namespace SImpl.SearchModule.ElasticSearch.Application.Services.SubQueries
{
    public class NumericRangeQueryElasticTranslator : ISubQueryElasticTranslator<NumericRange>
    {
        public QueryContainerDescriptor<TViewModel> Translate<TViewModel>(
            IEnumerable<ISubQueryElasticTranslator> collection, ISearchSubQuery query) where TViewModel : class
        {
            var castedQuery = (NumericRange)query;
            var queryResult = new QueryContainerDescriptor<TViewModel>();
      
                queryResult.Range(x =>
                {
                    var range = new NumericRangeQueryDescriptor<TViewModel>();
                    range= range.Field( new Field(castedQuery.Field));
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