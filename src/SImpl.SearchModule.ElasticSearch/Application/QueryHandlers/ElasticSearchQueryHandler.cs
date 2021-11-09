using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using SImpl.CQRS.Queries;
using SImpl.SearchModule.Abstraction.Handlers;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;
using SImpl.SearchModule.ElasticSearch.Application.Services;
using SImpl.SearchModule.ElasticSearch.Models;
using Filter = SImpl.SearchModule.Abstraction.Models.Filter;

namespace SImpl.SearchModule.ElasticSearch.Application.QueryHandlers
{
    public class ElasticSearchQueryHandler : IQueryHandler<SearchQuery,IQueryResult>
    {
        private IElasticSearchQueryTranslatorService _translatorService;
        private readonly IElasticClient _client;

        public ElasticSearchQueryHandler(IElasticSearchQueryTranslatorService translatorService, IElasticClient client)
        {
            _translatorService = translatorService;
            _client = client;
        }

        public async Task<IQueryResult> HandleAsync(SearchQuery query)
        {
            SearchDescriptor<ISearchModel> searchDescriptor = _translatorService.Translate(query);
            var  index =await _client.Indices.ExistsAsync(query.Index.ToLowerInvariant());
            if (!index.Exists)
            {
               return   new SimplQueryResult()
               {
                   SearchModels = new List<ISearchModel>(),
                   Total = 0,
                   Page = query.Page
               };
            }
            var result = await _client.SearchAsync<ElasticSearchModel>(s => searchDescriptor.Index(query.Index.ToLowerInvariant()));
            var resultModel = new SimplQueryResult()
            {
                SearchModels = result.Documents.Select(ElasticSearchModelMapper.Map).ToList(),
                Facets = TranslateFacets(result.Aggregations),
                Filters = TranslateFilters(result.Aggregations),
                Total = result.Total,
                Page = query.Page
            };
            return resultModel;
        }

        private List<BaseFacet> TranslateFacets(AggregateDictionary resultAggregations)
        {
            var list = new List<BaseFacet>();
            if (resultAggregations.ContainsKey("facets")) //todo: probably worth to move somewhereelse
            {
                var facetAggr = (BucketAggregate)resultAggregations["facets"];
                foreach (var bucket in facetAggr.Items.Select(x=>x as KeyedBucket<object>))
                {
                    list.Add(new BaseFacet()
                    {
                        Key = bucket.Key.ToString()
                    });
                }
            }

            return list;
        }
        private List<Filter> TranslateFilters(AggregateDictionary resultAggregations)
        {
            var list = new List<Filter>();
            if (resultAggregations.ContainsKey("filters")) //todo: probably worth to move somewhereelse
            {
                var facetAggr = (BucketAggregate)resultAggregations["filters"];
                foreach (var bucket in facetAggr.Items.Select(x=>x as KeyedBucket<object>))
                {
                    list.Add(new Filter()
                    {
                        Id = bucket.Key.ToString()
                    });
                }
            }

            return list;
        }
    }
}