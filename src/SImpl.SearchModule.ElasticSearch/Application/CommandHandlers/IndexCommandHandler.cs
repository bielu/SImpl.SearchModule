using System.Threading.Tasks;
using Nest;
using SImpl.CQRS.Commands;
using SImpl.SearchModule.Abstraction.Commands;
using SImpl.SearchModule.Abstraction.Models;

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
            await _client.BulkAsync(x => x.IndexMany<ISearchModel>(command.Models, (bulkDes, record) => bulkDes
                .Index(command.Index)
                .Document(record)));
        }
    }
}