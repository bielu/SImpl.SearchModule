using System.Linq;
using System.Threading.Tasks;
using Examine;
using Examine.Search;
using Newtonsoft.Json;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;
using SImpl.SearchModule.Examine.Application.Services;

namespace SImpl.SearchModule.Examine.Application.QueryHandlers
{
    public class ElasticSearchQueryHandler : IElasticSearchQueryHandler
    {
        private readonly IExamineQueryTranslatorService _translatorService;
        private readonly IExamineManager _examineManager;

        public ElasticSearchQueryHandler(IExamineQueryTranslatorService translatorService, IExamineManager examineManager)
        {
            _translatorService = translatorService;
            _examineManager = examineManager;
        }

        public async Task<IQueryResult> HandleAsync(ISearchQuery<IQueryResult> query)
        {
            _examineManager.TryGetIndex(query.Index, out IIndex index);
            
            IBooleanOperation searchDescriptor = _translatorService.Translate(query, out ISearcher searcher);
           var result= searchDescriptor.Execute(new QueryOptions(query.PageSize*(query.Page-1), query.PageSize));
            var resultModel = new SimplQueryResult()
            {
                //defaultOption but can be override in custom handlers
                SearchModels = result.Select(x=>JsonConvert.DeserializeObject<ISearchModel>(x.Values["ViewModel"])).ToList(),
                Total = result.TotalItemCount,
                Page = query.Page
            };
            return resultModel;
        }
    }
}