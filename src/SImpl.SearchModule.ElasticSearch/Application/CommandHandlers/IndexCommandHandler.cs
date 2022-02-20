using System;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using SImpl.CQRS.Commands;
using SImpl.SearchModule.Abstraction.Commands;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.ElasticSearch.Configuration;
using SImpl.SearchModule.ElasticSearch.Models;

namespace SImpl.SearchModule.ElasticSearch.Application.CommandHandlers
{
    public class IndexCommandHandler : ICommandHandler<IndexCommand>
    {
        private readonly IElasticClient _client;
        private readonly ElasticSearchConfiguration _elasticSearchConfiguration;

        public IndexCommandHandler(IElasticClient client, ElasticSearchConfiguration elasticSearchConfiguration)
        {
            _client = client;
            _elasticSearchConfiguration = elasticSearchConfiguration;
        }

        public async Task HandleAsync(IndexCommand command)
        {
            var indexAlias =_elasticSearchConfiguration.IndexPrefixName+ command.Index.ToLowerInvariant();
            var indexName = ( _elasticSearchConfiguration.UseZeroDowntimeIndexing
                ? indexAlias
                : indexAlias + DateTime.Now.ToString("-dd-MMM-HH-mm-ss"));

            var index = await _client.Indices.ExistsAsync(indexAlias);
            if (!index.Exists)
            {
                var answer = await _client.Indices.CreateAsync(indexName, index =>
                {
                    if (_elasticSearchConfiguration.UseZeroDowntimeIndexing)
                    {
                        index = index.Aliases(x => x.Alias(indexAlias));
                    }

                    return index.Map(f => f.Properties<ElasticSearchModel>(ps => ps
                        .Keyword(s => s
                            .Name(n => n.Id)
                        )
                        .Keyword(s => s
                            .Name(n => n.ContentType)
                        ).Keyword(s => s
                            .Name(n => n.Facet)
                        ).Keyword(s => s
                            .Name(n => n.Tags)
                        )).AutoMap<ElasticSearchModel>());
                });
            }

            var answerIndex = await _client.BulkAsync(x =>
                x.IndexMany<ElasticSearchModel>(command.Models.Select(ElasticSearchModelMapper.Map)
                    .ToList(), (bulkDes, record) => bulkDes
                    .Index(indexAlias)
                    .Document(record)));
            if (answerIndex.Errors)
            {
                throw new Exception(answerIndex.DebugInformation);
            }
        }
    }
}