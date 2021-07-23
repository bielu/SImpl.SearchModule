using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Nest;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.ElasticSearch.Application.Services.SubQueries;

namespace SImpl.SearchModule.ElasticSearch.Application.Services
{
    public class BaseElasticSearchQueryTranslatorService : IElasticSearchQueryTranslatorService
    {
        private IEnumerable<ISubQueryElasticTranslator> _collection;

        public BaseElasticSearchQueryTranslatorService(IEnumerable<ISubQueryElasticTranslator> collection)
        {
            _collection = collection;
        }

        public SearchDescriptor<ISearchModel> Translate<T>(ISearchQuery<T> query)
            where T : class
        {
            SearchDescriptor<ISearchModel> translated = new SearchDescriptor<ISearchModel>();
            if (query.FacetFields.Any())
            {
                var facets = new AggregationContainerDescriptor<ISearchModel>();
                foreach (var facetField in query.FacetFields)
                {
                    facets = facets.Terms("facets", t => t.Field(facetField.FieldName));
                }

                translated = translated.Aggregations(f => facets);
            }

            if (query.Any())
            {
                var queryContainer = new BoolQueryDescriptor<ISearchModel>();
                var mustQueries = new List<QueryContainer>();
                var shouldQueries = new List<QueryContainer>();
                var filterQueries = new List<QueryContainer>();
                var mustNotQueries = new List<QueryContainer>();

                foreach (var field in query)
                {
                    var type = field.Value.GetType();
                    var handlerType =
                        typeof(ISubQueryElasticTranslator<>).MakeGenericType(typeof(ISubQueryElasticTranslator<>),
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
                            filterQueries.Add(translator.Translate<ISearchModel>(_collection,field.Value));
                            break;
                        case Occurance.Must:
                            mustQueries.Add(translator.Translate<ISearchModel>(_collection,field.Value));
                            break;
                        case Occurance.MustNot:
                            mustNotQueries.Add(translator.Translate<ISearchModel>(_collection,field.Value));
                            break;
                        case Occurance.Should:
                            shouldQueries.Add(translator.Translate<ISearchModel>(_collection,field.Value));
                            break;
                    }
                }

                queryContainer.Filter(filterQueries.ToArray());
                queryContainer.MustNot(shouldQueries.ToArray());
                queryContainer.Must(mustQueries.ToArray());
                queryContainer.Should(mustNotQueries.ToArray());
                translated = translated.Query(x => x.Bool(b => queryContainer));
            }

            translated = translated.Size(query.PageSize);
            translated = translated.Skip(query.Page * query.PageSize);
            translated = translated.Index(query.Index);
            return translated;
        }
    }
}