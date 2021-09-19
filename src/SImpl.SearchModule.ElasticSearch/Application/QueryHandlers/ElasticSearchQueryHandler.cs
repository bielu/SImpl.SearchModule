using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;
using SImpl.SearchModule.ElasticSearch.Application.Services;
using SImpl.SearchModule.ElasticSearch.Models;

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
            var  index =await _client.Indices.ExistsAsync(query.Index.ToLowerInvariant());
            if (!index.Exists)
            {
               return   new SimplQueryResult()
               {
                   SearchModels = new List<ISearchModel>(),
                   Total = 0,
                   Page = query.Page
               };
            }
            var result = await _client.SearchAsync<ElasticSearchModel>(s => searchDescriptor.Index(query.Index.ToLowerInvariant()));
            var resultModel = new SimplQueryResult()
            {
                SearchModels = result.Documents.Select(ElasticSearchModelMapper.Map).ToList(),
                Total = result.Total,
                Page = query.Page
            };
            return resultModel;
        }
    }
}