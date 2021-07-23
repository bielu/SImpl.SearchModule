using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Examine;
using Newtonsoft.Json;
using SImpl.CQRS.Commands;
using SImpl.SearchModule.Abstraction.Commands;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Examine.Configuration;

namespace SImpl.SearchModule.Examine.Application.CommandHandlers
{
    public class IndexCommandHandler : ICommandHandler<IndexCommand>
    {
        private readonly IExamineManager _examineManager;

        public IndexCommandHandler(IExamineManager examineManager)
        {
            _examineManager = examineManager;
        }

        public async Task HandleAsync(IndexCommand command)
        {
            _examineManager.TryGetIndex(command.Index, out IIndex index);
            if (!index.IndexExists())
            {
                index.CreateIndex();
            }

            index.IndexItems(CreateValueSet(command.Models));
        }

        private IEnumerable<ValueSet> CreateValueSet(List<ISearchModel> commandModels)
        {
            List<ValueSet> list = new List<ValueSet>();
            foreach (var searchModel in commandModels)
            {
                var valueSet = new ValueSet(searchModel.ContentKey, "SImplSearch", new Dictionary<string, object>()
                {
                    {"ViewModel", JsonConvert.SerializeObject(searchModel)},
                    {nameof(searchModel.Culture), searchModel.Culture},
                    {nameof(searchModel.Content), searchModel.Content},
                    {nameof(searchModel.ContentType), searchModel.ContentType},
                    {nameof(searchModel.ContentKey), searchModel.ContentKey},
                    {nameof(searchModel.IndexedAt), searchModel.IndexedAt},
                    {nameof(searchModel.ViewModelType), searchModel.ViewModelType},
                    {nameof(searchModel.Tags), searchModel.Tags},

                });
                foreach (var property in searchModel.CustomProperties)
                {
                    valueSet.Add(nameof(searchModel.CustomProperties)+"."+property.Key, searchModel.ContentType);
                }

                list.Add(valueSet);
            }

            return list;
        }
    }
}