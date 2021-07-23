using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;
using SImpl.SearchModule.ElasticSearch.Application.Services;

namespace SImpl.SearchModule.ElasticSearch.Application.QueryHandlers
{
    public class ElasticSearchQueryHandler : IElasticSearchQueryHandler
    {
        private IElasticSearchQueryTranslatorService _translatorService;
        private readonly IElasticClient _client;

        public ElasticSearchQueryHandler(IElasticSearchQueryTranslatorService translatorService, IElasticClient client)
        {
            _translatorService = translatorService;
            _client = client;
        }

        public async Task<IQueryResult> HandleAsync(ISearchQuery<IQueryResult> query)
        {
            SearchDescriptor<ISearchModel> searchDescriptor = _translatorService.Translate(query);

            var result = await _client.SearchAsync<ISearchModel>(s => searchDescriptor);
            var resultModel = new SimplQueryResult()
            {
                SearchModels = result.Documents.ToList(),
                Total = result.Total,
                Page = query.Page
            };
            return resultModel;
        }
    }
}