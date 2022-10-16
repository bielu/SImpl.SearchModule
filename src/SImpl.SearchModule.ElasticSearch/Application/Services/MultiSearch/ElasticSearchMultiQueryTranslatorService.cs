using Nest;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.ElasticSearch.Models;

namespace SImpl.SearchModule.ElasticSearch.Application.Services.MultiSearch
{
    public class ElasticSearchMultiQueryTranslatorService : IElasticSearchMultiQueryTranslatorService
    {
        private readonly IElasticSearchQueryTranslatorService _elasticSearchQueryTranslatorService;

        public ElasticSearchMultiQueryTranslatorService(IElasticSearchQueryTranslatorService elasticSearchQueryTranslatorService)
        {
            _elasticSearchQueryTranslatorService = elasticSearchQueryTranslatorService;
        }

        public MultiSearchDescriptor Translate(MultiSearchQuery query)
        {
            var descriptor = new MultiSearchDescriptor();
            foreach (var keyedQuery in query.Queries)
            {
                descriptor.Search<ISearchModel>(keyedQuery.Key,
                    n => _elasticSearchQueryTranslatorService.Translate<ISearchModel>(keyedQuery.Value));

            }

            return descriptor;
        }
    }
}