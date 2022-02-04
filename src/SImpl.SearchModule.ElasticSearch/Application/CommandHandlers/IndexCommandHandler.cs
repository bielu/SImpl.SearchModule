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

        public IndexCommandHandler(IElasticClient client)
        {
            _client = client;
        }

        public async Task HandleAsync(IndexCommand command)
        {
            var  index =await _client.Indices.ExistsAsync(command.Index.ToLowerInvariant());
            if (!index.Exists)
            {
                var answer = await _client.Indices.CreateAsync(command.Index.ToLowerInvariant(), index=>index.Map(f=>f.AutoMap<ElasticSearchModel>().Properties<ElasticSearchModel>(ps => ps
                    .Keyword(s => s
                        .Name(n => n.ContentType)
                    ) .Keyword(s => s
                        .Name(n => n.Facet)
                    ).Keyword(s => s
                        .Name(n => n.Tags)
                    ))));
            }
           var answerIndex= await _client.BulkAsync(x => 
               x.IndexMany<ElasticSearchModel>(command.Models.Select(ElasticSearchModelMapper.Map)
                .ToList(), (bulkDes, record) => bulkDes
                .Index(command.Index.ToLowerInvariant())
                .Document(record)));
            if (answerIndex.Errors)
            {
                throw new Exception(answerIndex.DebugInformation);
            }
        }
    }
}