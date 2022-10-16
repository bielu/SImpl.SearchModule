using Algolia.Search.Clients;
using SImpl.CQRS.Commands;
using SImpl.SearchModule.Abstraction.Commands;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Algolia.Configuration;

namespace SImpl.SearchModule.Algolia.Application.CommandHandlers
{
    public class IndexCommandHandler : ICommandHandler<IndexCommand>
    {
        private readonly ISearchClient _algoliaSearchClient;
        private readonly AlgoliaSearchConfiguration _configuration;
        private readonly ILogger<IndexCommandHandler> _logger;


        public IndexCommandHandler(ISearchClient algoliaSearchClient, AlgoliaSearchConfiguration configuration,
            ILogger<IndexCommandHandler> logger)
        {
            _algoliaSearchClient = algoliaSearchClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task HandleAsync(IndexCommand command)
        {
            var indexName = _configuration.IndexPrefixName + command.Index.ToLowerInvariant();
           var index=  _algoliaSearchClient.InitIndex(indexName);
           var result = await index.SaveObjectsAsync(command.Models);
        }
        
    }
}