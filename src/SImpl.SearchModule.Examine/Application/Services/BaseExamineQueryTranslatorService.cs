using System.Collections.Generic;
using System.Linq;
using Examine;
using Examine.Search;
using SImpl.SearchModule.Abstraction.Fields;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.HighlightQueries;
using SImpl.SearchModule.Examine.Application.Services.FacetQueries;
using SImpl.SearchModule.Examine.Application.Services.SubQueries;

namespace SImpl.SearchModule.Examine.Application.Services
{
    public class BaseExamineQueryTranslatorService : IExamineQueryTranslatorService
    {
        private IEnumerable<ISubQueryElasticTranslator> _collection;
        private readonly IEnumerable<IFacetElasticTranslator> _facetElasticTranaslators;
        private readonly IExamineManager _examineManager;

        public BaseExamineQueryTranslatorService(IEnumerable<ISubQueryElasticTranslator> collection,
            IEnumerable<IFacetElasticTranslator> facetElasticTranaslators, IExamineManager examineManager)
        {
            _collection = collection;
            _facetElasticTranaslators = facetElasticTranaslators;
            _examineManager = examineManager;
        }

        public IQuery Translate<T>(IIndex index, ISearchQuery<T> query)
            where T : class
        {
            var searcher = index.Searcher;
            IQuery translatedQuery = searcher.CreateQuery();
            translatedQuery.All().Execute()
           
        }

       
    }
}