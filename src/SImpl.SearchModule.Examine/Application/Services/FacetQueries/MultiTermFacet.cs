using System;
using System.Collections.Generic;
using System.Linq;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Examine.Application.Services.SubQueries;

namespace SImpl.SearchModule.Examine.Application.Services.FacetQueries
{
    public class MultiTermFacetTranslator : IFacetElasticTranslator<MultiTermAggreation>
    {
        public IBucketAggregation PrepareAggregation( IAggregationQuery facetField, IEnumerable<ISubQueryElasticTranslator> collection  )
        {
            var multiTermAggreation = facetField as MultiTermAggreation;
            var term = new MultiTermsAggregationDescriptor<ISearchModel>();
            var termFacet = facetField as TermAggregation;
            term.Terms(multiTermAggreation.Terms.Select(x =>
                new Func<TermDescriptor<ISearchModel>, ITerm>(e => e.Field(x.TermFieldName))));
            return term;
        }
        public void PrepareNestedAggregation(IBucketAggregation bucket,IAggregationQuery subquery, IEnumerable<ISubQueryElasticTranslator> translators ,Func<AggregationContainerDescriptor<ISearchModel>, IAggregationQuery, IEnumerable<ISubQueryElasticTranslator>,AggregationContainerDescriptor<ISearchModel> > generatesubQuery )
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

            facets.MultiTerms(facetField.AggregationName, e => descriptor);
            
          
        }
    }
}