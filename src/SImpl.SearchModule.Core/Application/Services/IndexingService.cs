using System;
using System.Collections.Generic;
using SImpl.CQRS.Commands;
using SImpl.SearchModule.Abstraction.Commands;
using SImpl.SearchModule.Abstraction.Models;

namespace SImpl.SearchModule.Core.Application.Services
{
    public class IndexingService : IIndexingService
    {
        private readonly IInMemoryCommandDispatcher _commandDispatcher;

        public IndexingService(IInMemoryCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        
        public void Index(List<ISearchModel> searchModels, string index)
        {
            _commandDispatcher.ExecuteAsync(new IndexCommand()
            {
                Index = index,
                Models = searchModels
            });
        }
        public void Delete(List<Guid> searchModels, string index)
        {
            _commandDispatcher.ExecuteAsync(new RemoveCommand()
            {
                Index = index,
                ModelsIds = searchModels
            });
        }
        public void Delete(List<string> searchModels, string index)
        {
            _commandDispatcher.ExecuteAsync(new RemoveCommand()
            {
                Index = index,
                ModelsKeys = searchModels
            });
        }

        public void Delete(List<ISearchModel> searchModels, string index)
        {
            _commandDispatcher.ExecuteAsync(new RemoveCommand()
            {
                Index = index,
                Models = searchModels
            });
        }
    }
}