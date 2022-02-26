using System;
using System.Collections.Generic;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Examine.Application.Services.SubQueries;
using TermAggregation = SImpl.SearchModule.Abstraction.Queries.TermAggregation;

namespace SImpl.SearchModule.Examine.Application.Services.FacetQueries
{
    public class TermFacetTranslator : IFacetElasticTranslator<TermAggregation>
    {
        public IBucketAggregation PrepareAggregation( IAggregationQuery facetField, IEnumerable<ISubQueryElasticTranslator> translators )
        {
            var term = new TermsAggregationDescriptor<ISearchModel>();
            var termFacet = facetField as TermAggregation;
            term.Field(termFacet.TermFieldName);
            return term;
        }

        public void PrepareNestedAggregation(IBucketAggregation bucket,IAggregationQuery subquery, IEnumerable<ISubQueryElasticTranslator> translators ,Func<AggregationContainerDescriptor<ISearchModel>, IAggregationQuery, IEnumerable<ISubQueryElasticTranslator>,AggregationContainerDescriptor<ISearchModel> > generatesubQuery )
        {
            var descriptor = bucket as TermsAggregationDescriptor<ISearchModel>;
            var nested = new AggregationContainerDescriptor<ISearchModel>();
            descriptor.Aggregations(e => generatesubQuery.Invoke(nested, subquery, translators));
     
        }
        public void Translate(AggregationContainerDescriptor<ISearchModel> facets, IAggregationQuery facetField,
            IBucketAggregation bucket)
        {
          
            var descriptor = bucket as TermsAggregationDescriptor<ISearchModel>;

            facets.Terms(facetField.AggregationName, e => descriptor);
        }

    
    }
}