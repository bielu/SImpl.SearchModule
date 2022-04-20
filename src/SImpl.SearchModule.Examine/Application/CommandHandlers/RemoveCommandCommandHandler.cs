using System;
using System.Linq;
using System.Threading.Tasks;
using Examine;
using Microsoft.Extensions.Logging;
using SImpl.CQRS.Commands;
using SImpl.SearchModule.Abstraction.Commands;
using SImpl.SearchModule.Examine.Application.Services;
using SImpl.SearchModule.Examine.Configuration;
using SImpl.SearchModule.Examine.Models;

namespace SImpl.SearchModule.Examine.Application.CommandHandlers
{
    public class RemoveCommandCommandHandler : ICommandHandler<RemoveCommand>
    {
        private readonly IExamineManager _examineManager;
        private readonly ExamineSearchConfiguration _configuration;
        private readonly ILogger<IndexCommandHandler> _logger;

        public RemoveCommandCommandHandler(IExamineManager examineManager, ExamineSearchConfiguration configuration,
            ILogger<IndexCommandHandler> logger)
        {
            _examineManager = examineManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task HandleAsync(RemoveCommand command)
        {
            var indexName = _configuration.IndexPrefixName + command.Index.ToLowerInvariant();
            _examineManager.TryGetIndex(indexName,
                out IIndex examineIndex);
            if (examineIndex == null)
            {
                _logger.LogError($"Examine index not found {indexName}");
                return;
            }

            if (command.Models.Any())
            {
                try
                {   
                    _logger.LogInformation($"Deleted items {string.Join(", ",command.Models.Select(x=>x.Id.ToString()).ToList())}");
                examineIndex.DeleteFromIndex(command.Models.Select(x=>x.Id));
                     
                }
                catch (Exception e)
                {
                    _logger.LogError($"remove from index {indexName} failed");
                }
            }
            else if (command.ModelsIds.Any())
            {
                try
                {       _logger.LogInformation($"Deleted items {string.Join(", ",command.ModelsIds)}");

                    examineIndex.DeleteFromIndex(command.ModelsIds.Select(x=>x.ToString()));

                }
                catch (Exception e)
                {
                    _logger.LogError($"remove from index {indexName} failed");
                }
          
            }else if (command.ModelsKeys.Any())
            {
                try
                {       _logger.LogInformation($"Deleted items {string.Join(", ",command.ModelsKeys)}");

                    examineIndex.DeleteFromIndex(command.ModelsKeys);

                }
                catch (Exception e)
                {
                    _logger.LogError($"remove from index {indexName} failed");
                }
               
            }
          
        }
    }
}