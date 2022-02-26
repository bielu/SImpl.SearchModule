using System.Collections.Generic;
using Examine.Lucene.Search;
using Examine.Search;
using Lucene.Net.Search;
using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.Examine.Application.Services.SubQueries
{
    public interface ISubQueryElasticTranslator<T> : ISubQueryElasticTranslator where T : ISearchSubQuery
    {
     
    }

    public interface ISubQueryElasticTranslator
    {
        public Query Translate<TViewModel>(IEnumerable<ISubQueryElasticTranslator> collection,ISearchSubQuery query) where TViewModel : class;
    }
}