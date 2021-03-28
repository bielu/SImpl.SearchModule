using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;
using SImpl.SearchModule.ElasticSearch.Application.Commands;

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
            QueryContainer elasticQuery = _translatorService.Translate(query);

           var result=  _client.Search<IQueryResult>(s => s.Query(e => elasticQuery));
           return result.Documents.FirstOrDefault();
        }
    }
}