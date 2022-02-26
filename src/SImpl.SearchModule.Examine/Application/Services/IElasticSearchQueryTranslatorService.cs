using Examine;
using Examine.Search;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.Examine.Application.Services
{
    public interface IExamineQueryTranslatorService
    {
        public IQuery Translate<T>(IIndex index,ISearchQuery<T> query) where T : class;
    }
}