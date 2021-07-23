using Examine;
using Examine.Search;
using Lucene.Net.Search;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;

namespace SImpl.SearchModule.Examine.Application.Services
{
    public interface IExamineQueryTranslatorService
    {
        public IBooleanOperation Translate<T>(ISearchQuery<T> query, out ISearcher searcher) where T : class;
    }
}