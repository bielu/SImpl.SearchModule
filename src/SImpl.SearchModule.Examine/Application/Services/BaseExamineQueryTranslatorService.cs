using System.Collections.Generic;
using System.Linq;
using Examine;
using Examine.Lucene.Search;
using Examine.Search;
using Lucene.Net.Search;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Examine.Application.Services.SubQueries;

namespace SImpl.SearchModule.Examine.Application.Services
{
    public class BaseExamineQueryTranslatorService : IExamineQueryTranslatorService
    {
        private IEnumerable<ISubQueryExamineTranslator> _collection;
        private readonly IExamineManager _manager;

        public BaseExamineQueryTranslatorService(IEnumerable<ISubQueryExamineTranslator> collection, IExamineManager manager)
        {
            _collection = collection;
            _manager = manager;
        }

        public IBooleanOperation Translate<T>(ISearchQuery<T> query, out ISearcher searcher1)
            where T : class
        {
            IQuery queryResult = null;
            _manager.TryGetIndex(query.Index, out IIndex index);
            if (index == null)
            {
                searcher1 = null;
                return null;
            }
            var searcher = index.Searcher;
            if (searcher == null)
            {
                searcher1 = null;
                return null;
            }

            searcher1 = searcher;
            var queryBase = searcher.CreateQuery();
            var queryContainer = new LuceneBooleanOperation(queryBase as LuceneSearchQuery);
            //todo: figure out how to use Lucene.net Facets
            /*if (query.FacetFields.Any())
            {
                var facets = new AggregationContainerDescriptor<ISearchModel>();
                foreach (var facetField in query.FacetFields)
                {
                    facets = facets.Terms("facets", t => t.Field(facetField.FieldName));
                }

                tempQuery = tempQuery(f => facets);
            }*/
               if (query.Any())
            {
      
                foreach (var field in query)
                {
                    var type = field.Value.GetType();
                    var handlerType =
                        typeof(ISubQueryExamineTranslator<>).MakeGenericType(typeof(ISubQueryExamineTranslator<>),
                            type);
                    var translator =
                        _collection.FirstOrDefault(x => x.GetType().GetGenericTypeDefinition() == handlerType);
                    if (translator == null)
                    {
                        continue;
                    }

                    switch (field.Key)
                    {
                        case Occurance.Filter:
                            queryContainer.And( e=>translator.Translate<ISearchModel>(_collection,field.Value,(IQuery)e));
                            break;
                        case Occurance.Must:
                            queryContainer.And(e=>translator.Translate<ISearchModel>(_collection,field.Value, (IQuery)e));
                            break;
                        case Occurance.MustNot:
                            queryContainer.AndNot(e=>translator.Translate<ISearchModel>(_collection,field.Value,(IQuery) e));
                            break;
                        case Occurance.Should:
                            queryContainer.And(e=>translator.Translate<ISearchModel>(_collection,field.Value,(IQuery) e),BooleanOperation.Or);
                            break;
                    }
                }

                
            }

            return queryContainer;
        }
    }
}