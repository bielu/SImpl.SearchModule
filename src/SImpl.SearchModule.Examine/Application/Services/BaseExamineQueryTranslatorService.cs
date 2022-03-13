using System.Collections.Generic;
using System.Linq;
using Examine;
using Examine.Search;
using SImpl.SearchModule.Abstraction.Fields;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.HighlightQueries;
using SImpl.SearchModule.Examine.Application.LuceneEngine;
using SImpl.SearchModule.Examine.Application.Services.SubQueries;

namespace SImpl.SearchModule.Examine.Application.Services
{
    public class BaseExamineQueryTranslatorService : IExamineQueryTranslatorService
    {
        private IEnumerable<ISubQueryElasticTranslator> _collection;
        private readonly IExamineManager _examineManager;

        public BaseExamineQueryTranslatorService(IEnumerable<ISubQueryElasticTranslator> collection, IExamineManager examineManager)
        {
            _collection = collection;
            _examineManager = examineManager;
        }

        public IQuery Translate<T>(IIndex index, ISearchQuery<T> query)
            where T : class
        {
            var searcher = index.Searcher;
            IQuery translatedQuery = searcher.CreateQuery();
            var nestedQuery = translatedQuery as LuceneSearchQueryWithFiltersAndFacets;

            foreach (var booleanQuery in query)
            {
                var type = booleanQuery.Value.GetType();
                var handlerType =
                    typeof(ISubQueryExamineTranslator<>).MakeGenericType(type);

                var translator =
                    _collection.FirstOrDefault(x => x.GetType().GetInterfaces().Any(x => x == handlerType));
                if (translator == null)
                {
                    continue;
                }
                switch (booleanQuery.Key)
                {
                    case Occurance.MustNot:
                        nestedQuery.Not(translator.Translate<T>(searcher,_collection, booleanQuery.Value));
                        break;
                    case Occurance.Must:
                        nestedQuery.And(translator.Translate<T>(searcher,_collection, booleanQuery.Value));
                        break;
                    case Occurance.Should:
                        nestedQuery.Or(translator.Translate<T>(searcher,_collection, booleanQuery.Value));
                        break;
                }
            }

            return translatedQuery;

        }

       
    }
}