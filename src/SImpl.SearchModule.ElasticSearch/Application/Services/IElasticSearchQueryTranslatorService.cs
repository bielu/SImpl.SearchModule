using Nest;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.ElasticSearch.Application.Services
{
    public interface IElasticSearchQueryTranslatorService
    {
        public SearchDescriptor<ISearchModel> Translate<T>(ISearchQuery<T> query) where T : class;
    }
}