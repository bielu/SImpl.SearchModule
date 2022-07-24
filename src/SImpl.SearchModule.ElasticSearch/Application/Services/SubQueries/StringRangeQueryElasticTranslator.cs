using System.Collections.Generic;
using Nest;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;

namespace SImpl.SearchModule.ElasticSearch.Application.Services.SubQueries
{
    public class StringRangeQueryElasticTranslator : ISubQueryElasticTranslator<StringRange>
    {
        public QueryContainerDescriptor<TViewModel> Translate<TViewModel>(
            IEnumerable<ISubQueryElasticTranslator> collection, ISearchSubQuery query) where TViewModel : class
        {
            var castedQuery = (StringRange)query;
            var queryResult = new QueryContainerDescriptor<TViewModel>();
      
            queryResult.TermRange(x =>
            {
                var range = new TermRangeQueryDescriptor<TViewModel>();
                range= range.Field( new Field(castedQuery.Field));
                if (castedQuery.IncludeMaxEdge && castedQuery.MaxValue != null)
                {
                    range=range.LessThanOrEquals(castedQuery.MaxValue);
                }
                else if(castedQuery.MaxValue != null)
                {
                    range=range.LessThan(castedQuery.MaxValue);
                }
                    
                if (castedQuery.IncludeMinEdge && castedQuery.MinValue!= null)
                {
                    range=range.LessThanOrEquals(castedQuery.MaxValue);
                }
                else if( castedQuery.MinValue != null)
                {
                    range=range.LessThan(castedQuery.MaxValue);
                }
                    
                return range;
            });
 
            
            return queryResult;
        }
    }
}