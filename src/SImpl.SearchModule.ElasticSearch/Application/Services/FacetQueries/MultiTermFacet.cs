using System;
using System.Collections.Generic;
using System.Linq;
using Nest;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.ElasticSearch.Application.Services.SubQueries;

namespace SImpl.SearchModule.ElasticSearch.Application.Services
{
    public class MultiTermFacetTranslator : IFacetElasticTranslator<MultiTermAggreation>
    {
        public IBucketAggregation PrepareAggregation(IAggregationQuery facetField,
            IEnumerable<ISubQueryElasticTranslator> collection)
        {
            var multiTermAggreation = facetField as MultiTermAggreation;
            var term = new MultiTermsAggregationDescriptor<ISearchModel>();
            var termFacet = facetField as TermAggregation;
            term.Terms(multiTermAggreation.Terms.Select(x =>
                new Func<TermDescriptor<ISearchModel>, ITerm>(e => e.Field(x.TermFieldName)))).Size(1000);
            return term;
        }

        public void PrepareNestedAggregation(IBucketAggregation bucket, IAggregationQuery subquery,
            IEnumerable<ISubQueryElasticTranslator> translators,
            Func<AggregationContainerDescriptor<ISearchModel>, IAggregationQuery,
                IEnumerable<ISubQueryElasticTranslator>, AggregationContainerDescriptor<ISearchModel>> generatesubQuery)
        {
            var descriptor = bucket as MultiTermsAggregationDescriptor<ISearchModel>;
            var nested = new AggregationContainerDescriptor<ISearchModel>();
            descriptor.Aggregations(e => generatesubQuery.Invoke(nested, subquery, translators));
        }

        public void Translate(AggregationContainerDescriptor<ISearchModel> facets, IAggregationQuery facetField,
            IBucketAggregation bucket)
        {
            var multiTermAggreation = facetField as MultiTermAggreation;
            var descriptor = bucket as MultiTermsAggregationDescriptor<ISearchModel>;
            descriptor.Terms(multiTermAggreation.Terms.Select(x =>
            {
                return new Term()
                {
                    Field = new Field(x.TermFieldName)
                };
            }).ToArray());
            
            facets.MultiTerms(facetField.AggregationName, e => descriptor);
        }
    }
}