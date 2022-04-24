using System;
using System.Collections.Generic;
using System.Linq;
using Nest;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.ElasticSearch.Application.Services.SubQueries;
using TermAggregation = SImpl.SearchModule.Abstraction.Queries.TermAggregation;

namespace SImpl.SearchModule.ElasticSearch.Application.Services
{
    public class FilterAggregation : IFacetElasticTranslator<FilterAggregationQuery>
    {
    
        public IBucketAggregation PrepareAggregation( IAggregationQuery facetField, IEnumerable<ISubQueryElasticTranslator> collection  )
        {
            var termFacet = facetField as FilterAggregationQuery;
            var term = new FilterAggregationDescriptor<ISearchModel>();
            term.Filter(f =>
            {
                SearchDescriptor<ISearchModel> translated = new SearchDescriptor<ISearchModel>();

                var queryContainer = new BoolQueryDescriptor<ISearchModel>();
                var mustQueries = new List<QueryContainer>();
                var shouldQueries = new List<QueryContainer>();
                var filterQueries = new List<QueryContainer>();
                var mustNotQueries = new List<QueryContainer>();
                if (!termFacet.Queries.Any())
                {
                    return f.MatchAll();
                }
                foreach (var field in termFacet.Queries)
                {
                    var type = field.Value.GetType();
                    var handlerType =
                        typeof(ISubQueryElasticTranslator<>).MakeGenericType(type);
                    var translator =
                        collection.FirstOrDefault(x => x.GetType().GetInterfaces().Any(x => x == handlerType));
                    if (translator == null)
                    {
                        continue;
                    }

                    switch (field.Key)
                    {
                        case Occurance.Filter:
                            filterQueries.Add(translator.Translate<ISearchModel>(collection, field.Value));
                            break;
                        case Occurance.Must:
                            mustQueries.Add(translator.Translate<ISearchModel>(collection, field.Value));
                            break;
                        case Occurance.MustNot:
                            mustNotQueries.Add(translator.Translate<ISearchModel>(collection, field.Value));
                            break;
                        case Occurance.Should:
                            shouldQueries.Add(translator.Translate<ISearchModel>(collection, field.Value));
                            break;
                    }
                }

                queryContainer.Filter(filterQueries.ToArray());
                queryContainer.MustNot(mustNotQueries.ToArray());
                queryContainer.Must(mustQueries.ToArray());
                queryContainer.Should(shouldQueries.ToArray());
                return f.Bool(b => queryContainer);
            });
            return term;
        }

        public void PrepareNestedAggregation(IBucketAggregation bucket, IAggregationQuery subquery, IEnumerable<ISubQueryElasticTranslator> translators,
            Func<AggregationContainerDescriptor<ISearchModel>, IAggregationQuery, IEnumerable<ISubQueryElasticTranslator>, AggregationContainerDescriptor<ISearchModel>> generatesubQuery)
        {
            var descriptor = bucket as FilterAggregationDescriptor<ISearchModel>;
            var nested = new AggregationContainerDescriptor<ISearchModel>();
            descriptor.Aggregations(e => generatesubQuery.Invoke(nested, subquery, translators));
     
        }
        public void Translate(AggregationContainerDescriptor<ISearchModel> facets, IAggregationQuery facetField,
            IBucketAggregation bucket)
        {
          
            var descriptor = bucket as FilterAggregationDescriptor<ISearchModel>;

            facets.Filter(facetField.AggregationName, e=> descriptor);
        }
    }
}