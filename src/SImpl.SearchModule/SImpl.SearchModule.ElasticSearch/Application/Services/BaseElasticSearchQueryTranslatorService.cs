using System.Linq;
using Nest;
using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.ElasticSearch.Application.Commands
{
    public class BaseElasticSearchQueryTranslatorService : IElasticSearchQueryTranslatorService
    {
        public  SearchDescriptor<T> Translate<T>(ISearchQuery<T> query)
        where T: class
        {
            SearchDescriptor<T> translated = new SearchDescriptor<T>();
            if (query.FacetFields.Any())
            {
                var facets = new AggregationContainerDescriptor<T>();
                foreach (var facetField in query.FacetFields)
                {
                    facets = facets.Terms("facets",t=>t.Field(facetField.FieldName));
                }
                translated= translated.Aggregations(f=>facets);
            }
            translated=  translated.Size(query.PageSize);
            translated=translated.Skip(query.Page * query.PageSize);
            return translated;
        }
    }
}