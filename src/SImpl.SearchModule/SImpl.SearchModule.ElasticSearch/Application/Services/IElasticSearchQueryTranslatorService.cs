using Nest;
using SImpl.CQRS.Commands;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;

namespace SImpl.SearchModule.ElasticSearch.Application.Commands
{
    public interface IElasticSearchQueryTranslatorService
    {
        public SearchDescriptor<T> Translate<T>(ISearchQuery<T> query) where T : class;
    }
}