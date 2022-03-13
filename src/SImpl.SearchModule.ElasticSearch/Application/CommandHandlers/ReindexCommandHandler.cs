using System;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using SImpl.CQRS.Commands;
using SImpl.SearchModule.Abstraction.Commands;
using SImpl.SearchModule.ElasticSearch.Configuration;
using SImpl.SearchModule.ElasticSearch.Models;

namespace SImpl.SearchModule.ElasticSearch.Application.CommandHandlers
{
    public class ReindexCommandHandler : ICommandHandler<ReIndexCommand>
    {
        private readonly IElasticClient _client;
        private readonly ElasticSearchConfiguration _elasticSearchConfiguration;

        public ReindexCommandHandler(IElasticClient client, ElasticSearchConfiguration elasticSearchConfiguration)
        {
            _client = client;
            _elasticSearchConfiguration = elasticSearchConfiguration;
        }

        public async Task HandleAsync(ReIndexCommand command)
        {
            //todo: move that to separete service
            var indexAlias =  _elasticSearchConfiguration.IndexPrefixName+command.Index.ToLowerInvariant();
            var indexName = (_elasticSearchConfiguration.UseZeroDowntimeIndexing
                ? indexAlias
                : indexAlias + DateTime.Now.ToString("-dd-MMM-HH-mm-ss")).ToLowerInvariant();
        
            var index = await _client.Indices.ExistsAsync(indexAlias);

            var answer = await _client.Indices.CreateAsync(indexName, index =>
            {
                return index.Map(f => f.AutoMap<ElasticSearchModel>().Properties<ElasticSearchModel>(ps => ps
                    .Keyword(s => s
                        .Name(n => n.ContentType)
                    ).Keyword(s => s
                        .Name(n => n.Facet)
                    ).Keyword(s => s
                        .Name(n => n.Tags)
                    )));
            });


            var answerIndex = await _client.BulkAsync(x =>
                x.IndexMany<ElasticSearchModel>(command.Models.Select(ElasticSearchModelMapper.Map)
                    .ToList(), (bulkDes, record) => bulkDes
                    .Index(indexAlias)
                    .Document(record)));
            var oldIndexes = await _client.Indices.GetAliasAsync(indexAlias);
            await _client.Indices.PutAliasAsync(answer.Index, indexAlias);

            if (oldIndexes.IsValid && oldIndexes.Indices.Any())
            {
                foreach (var oldIndex in oldIndexes.Indices)
                {
                    await _client.Indices.DeleteAsync(answer.Index);
                }
            }


            if (answerIndex.Errors)
            {
                throw new Exception(answerIndex.DebugInformation);
            }
        }
    }
}

