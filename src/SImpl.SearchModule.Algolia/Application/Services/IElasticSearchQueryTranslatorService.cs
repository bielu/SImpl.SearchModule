using Algolia.Search.Models.Search;
using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.Algolia.Application.Services
{
    public interface IAlgoliaQueryTranslatorService
    {
        public Query Translate<T>(IIndex index,ISearchQuery<T> query) where T : class;
    }
}