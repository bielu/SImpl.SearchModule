using Algolia.Search.Models.Search;
using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.Algolia.Application.Services.SubQueries
{
    public interface ISubQueryExamineTranslator<T> : ISubQueryElasticTranslator where T : ISearchSubQuery
    {
     
    }

    public interface ISubQueryElasticTranslator
    {
        public string Translate<TViewModel>(IEnumerable<ISubQueryElasticTranslator> collection,
            ISearchSubQuery query) where TViewModel : class;
    }
}