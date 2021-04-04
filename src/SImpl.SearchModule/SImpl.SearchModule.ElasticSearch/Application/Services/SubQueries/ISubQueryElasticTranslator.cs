using Nest;
using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.ElasticSearch.Application.Commands
{
    public interface ISubQueryElasticTranslator<T> : ISubQueryElasticTranslator where T : ISearchSubQuery
    {
     
    }

    public interface ISubQueryElasticTranslator
    {
        public QueryContainer Translate<TViewModel>(ISearchSubQuery query) where TViewModel : class;
    }
}