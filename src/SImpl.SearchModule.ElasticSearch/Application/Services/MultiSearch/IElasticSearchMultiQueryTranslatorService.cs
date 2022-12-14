using Nest;
using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.ElasticSearch.Application.Services.MultiSearch
{
    public interface IElasticSearchMultiQueryTranslatorService
    {
        MultiSearchDescriptor Translate(MultiSearchQuery query);
    }
}