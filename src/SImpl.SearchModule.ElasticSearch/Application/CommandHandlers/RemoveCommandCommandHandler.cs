using System;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using SImpl.CQRS.Commands;
using SImpl.SearchModule.Abstraction.Commands;
using SImpl.SearchModule.ElasticSearch.Models;

namespace SImpl.SearchModule.ElasticSearch.Application.CommandHandlers
{
    public class RemoveCommandCommandHandler : ICommandHandler<RemoveCommand>
    {
        private readonly IElasticClient _client;

        public RemoveCommandCommandHandler(IElasticClient client)
        {
            _client = client;
        }
        public async Task HandleAsync(RemoveCommand command)
        {
               var  index =await _client.Indices.ExistsAsync(command.Index.ToLowerInvariant());
            if (!index.Exists)
            {
                var answer = await _client.Indices.CreateAsync(command.Index.ToLowerInvariant(), index=>index.Map(f=>f.AutoMap<ElasticSearchModel>().Properties<ElasticSearchModel>(ps => ps
                    .Keyword(s => s
                        .Name(n => n.ContentType)
                    ).Keyword(s => s
                        .Name(n => n.Tags)
                    ))));
            }

            if (command.Models.Any())
            {
                var answerIndex= await _client.BulkAsync(x => 
                    x.DeleteMany<ElasticSearchModel>(command.Models.Select(ElasticSearchModelMapper.Map)
                        .ToList(), (bulkDes, record) => bulkDes
                        .Index(command.Index.ToLowerInvariant())
                        .Document(record)));
                if (answerIndex.Errors)
                {
                    throw new Exception(answerIndex.DebugInformation);
                } 
            }
            else if (command.ModelsIds.Any())
            {
                var answerIndex= await _client.BulkAsync(x => 
                    x.DeleteMany<ElasticSearchModel>(command.ModelsIds.Select(x=>new ElasticSearchModel()
                        {
                            Id = x.ToString()
                        })
                        .ToList(), (bulkDes, record) => bulkDes
                        .Index(command.Index.ToLowerInvariant())
                        .Document(record)));
                if (answerIndex.Errors)
                {
                    throw new Exception(answerIndex.DebugInformation);
                } 
            }else if (command.ModelsKeys.Any())
            {
                var answerIndex= await _client.BulkAsync(x => 
                    x.DeleteMany<ElasticSearchModel>(command.ModelsKeys.Select(x=>new ElasticSearchModel()
                        {
                        })
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
}