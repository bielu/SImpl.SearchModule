using System;
using System.Collections.Generic;
using Nest;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;
using DateRangeQuery = SImpl.SearchModule.Abstraction.Queries.subqueries.DateRangeQuery;

namespace SImpl.SearchModule.ElasticSearch.Application.Services.SubQueries
{
    public class SpatialQueryElasticTranslator : ISubQueryElasticTranslator<SpatialSearchQuery>
    {
        public QueryContainerDescriptor<TViewModel> Translate<TViewModel>(
            IEnumerable<ISubQueryElasticTranslator> collection, ISearchSubQuery query) where TViewModel : class
        {
            var castedQuery = (SpatialSearchQuery)query;
            var queryResult = new QueryContainerDescriptor<TViewModel>();

            queryResult.GeoDistance(x =>
            {
                var range = new GeoDistanceQueryDescriptor<TViewModel>()
                    .Field(new Field(castedQuery.Field))
                    .Distance($"{castedQuery.Distance}{castedQuery.UnitOfDistance}")
                    .Location(castedQuery.Lat, castedQuery.Lng);

                return range;
            });


            return queryResult;
        }
    }
}