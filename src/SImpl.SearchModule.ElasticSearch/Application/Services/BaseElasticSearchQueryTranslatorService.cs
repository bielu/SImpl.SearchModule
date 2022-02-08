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
        private readonly IEnumerable<IFacetElasticTranslator> _facetElasticTranaslators;

        public BaseElasticSearchQueryTranslatorService(IEnumerable<ISubQueryElasticTranslator> collection, IEnumerable<IFacetElasticTranslator> facetElasticTranaslators)
        {
            _collection = collection;
            _facetElasticTranaslators = facetElasticTranaslators;
        }

        public SearchDescriptor<ISearchModel> Translate<T>(ISearchQuery<T> query)
            where T : class
        {
            SearchDescriptor<ISearchModel> translated = new SearchDescriptor<ISearchModel>();
            if (query.FacetQueries.Any())
            {
                var facets = new AggregationContainerDescriptor<ISearchModel>();

                foreach (var facetField in query.FacetQueries)
                {
                    TranslateAggregation(facetField, _facetElasticTranaslators, facets);
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
                        typeof(ISubQueryElasticTranslator<>).MakeGenericType(type);
                    var translator =
                        _collection.FirstOrDefault(x => x.GetType().GetInterfaces().Any(x => x == handlerType));
                    if (translator == null)
                    {
                        continue;
                    }

                    switch (field.Key)
                    {
                        case Occurance.Filter:
                            filterQueries.Add(translator.Translate<ISearchModel>(_collection, field.Value));
                            break;
                        case Occurance.Must:
                            mustQueries.Add(translator.Translate<ISearchModel>(_collection, field.Value));
                            break;
                        case Occurance.MustNot:
                            mustNotQueries.Add(translator.Translate<ISearchModel>(_collection, field.Value));
                            break;
                        case Occurance.Should:
                            shouldQueries.Add(translator.Translate<ISearchModel>(_collection, field.Value));
                            break;
                    }
                }

                queryContainer.Filter(filterQueries.ToArray());
                queryContainer.MustNot(mustNotQueries.ToArray());
                queryContainer.Must(mustQueries.ToArray());
                queryContainer.Should(shouldQueries.ToArray());
                translated = translated.Query(x => x.Bool(b => queryContainer));
            }

            if (query.PostFilterQuery.Any())
            {
                var queryContainer = new BoolQueryDescriptor<ISearchModel>();
                var mustQueries = new List<QueryContainer>();
                var shouldQueries = new List<QueryContainer>();
                var filterQueries = new List<QueryContainer>();
                var mustNotQueries = new List<QueryContainer>();

                foreach (var field in query.PostFilterQuery)
                {
                    var type = field.Value.GetType();
                    var handlerType =
                        typeof(ISubQueryElasticTranslator<>).MakeGenericType(type);
                    var translator =
                        _collection.FirstOrDefault(x => x.GetType().GetInterfaces().Any(x => x == handlerType));
                    if (translator == null)
                    {
                        continue;
                    }

                    switch (field.Key)
                    {
                        case Occurance.Filter:
                            filterQueries.Add(translator.Translate<ISearchModel>(_collection, field.Value));
                            break;
                        case Occurance.Must:
                            mustQueries.Add(translator.Translate<ISearchModel>(_collection, field.Value));
                            break;
                        case Occurance.MustNot:
                            mustNotQueries.Add(translator.Translate<ISearchModel>(_collection, field.Value));
                            break;
                        case Occurance.Should:
                            shouldQueries.Add(translator.Translate<ISearchModel>(_collection, field.Value));
                            break;
                    }
                }

                queryContainer.Filter(filterQueries.ToArray());
                queryContainer.MustNot(mustNotQueries.ToArray());
                queryContainer.Must(mustQueries.ToArray());
                queryContainer.Should(shouldQueries.ToArray());
                translated = translated.PostFilter(x => x.Bool(b => queryContainer));
            }

            translated = translated.Size(query.PageSize);
            translated = translated.Skip((query.Page - 1) * query.PageSize);
            translated = translated.Index(query.Index.ToLowerInvariant());
            return translated;
        }

        private void TranslateAggregation(IAggregationQuery facetField, IEnumerable<IFacetElasticTranslator> facetElasticTranaslators, AggregationContainerDescriptor<ISearchModel> facets)
        {
            var type = facetField.GetType();
            var handlerType =
                typeof(IFacetElasticTranslator<>).MakeGenericType(type);
            var translator =
                _facetElasticTranaslators.FirstOrDefault(x => x.GetType().GetInterfaces().Any(x => x == handlerType));
            
            
            
               var descriptor =     translator.PrepareAggregation(facetField, _collection);
               
            foreach (var aggregation in facetField.NestedAggregations)
            {
                  translator.PrepareNestedAggregation(descriptor,aggregation, _collection, (
                      (containerDescriptor, query, translators) =>
                      {
                          var aggregation = new AggregationContainerDescriptor<ISearchModel>();
                          var type = query.GetType();
                          var handlerType =
                              typeof(IFacetElasticTranslator<>).MakeGenericType(type);
                          var translator =
                              _facetElasticTranaslators.FirstOrDefault(x => x.GetType().GetInterfaces().Any(x => x == handlerType));
                          var descriptor =     translator.PrepareAggregation(query, translators);
                          translator.Translate(aggregation, query,descriptor);
                          return aggregation;
                      } ));
            }
            translator.Translate(facets, facetField,descriptor);
            
        }
    }
}