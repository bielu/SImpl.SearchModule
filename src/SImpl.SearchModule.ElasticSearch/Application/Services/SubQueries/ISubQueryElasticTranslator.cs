using System.Collections.Generic;
using Nest;
using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.ElasticSearch.Application.Services.SubQueries
{
    public interface ISubQueryElasticTranslator<T> : ISubQueryElasticTranslator where T : ISearchSubQuery
    {
     
    }

    public interface ISubQueryElasticTranslator
    {
        public QueryContainerDescriptor<TViewModel> Translate<TViewModel>(IEnumerable<ISubQueryElasticTranslator> collection,ISearchSubQuery query) where TViewModel : class;
    }
}