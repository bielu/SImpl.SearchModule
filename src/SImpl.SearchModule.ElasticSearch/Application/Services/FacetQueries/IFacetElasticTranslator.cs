using System;
using System.Collections.Generic;
using Nest;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.ElasticSearch.Application.Services.SubQueries;

namespace SImpl.SearchModule.ElasticSearch.Application.Services
{//todo simplify translation, we need figure out better way
    public interface IFacetElasticTranslator<T> : IFacetElasticTranslator where T : class, IAggregationQuery
    {
  

    }

    public interface IFacetElasticTranslator
    {
        IBucketAggregation PrepareAggregation(
            IAggregationQuery facetField, IEnumerable<ISubQueryElasticTranslator> translators );

        public void PrepareNestedAggregation(IBucketAggregation bucket, IAggregationQuery subquery,
            IEnumerable<ISubQueryElasticTranslator> translators,
            Func<AggregationContainerDescriptor<ISearchModel>, IAggregationQuery,
                    IEnumerable<ISubQueryElasticTranslator>, AggregationContainerDescriptor<ISearchModel>>
                generatesubQuery);

        void Translate(AggregationContainerDescriptor<ISearchModel> facets, IAggregationQuery facetField,
            IBucketAggregation bucket);
    }
}