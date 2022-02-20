using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nest;
using SImpl.CQRS.Queries;
using SImpl.SearchModule.Abstraction.Handlers;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;
using SImpl.SearchModule.ElasticSearch.Application.Services;
using SImpl.SearchModule.ElasticSearch.Application.Services.Result;
using SImpl.SearchModule.ElasticSearch.Configuration;
using SImpl.SearchModule.ElasticSearch.Models;
using IAggregation = SImpl.SearchModule.Abstraction.Models.IAggregation;
using IBucket = Nest.IBucket;

namespace SImpl.SearchModule.ElasticSearch.Application.QueryHandlers
{
    public class ElasticSearchQueryHandler : IQueryHandler<SearchQuery, IQueryResult>
    {
        private IElasticSearchQueryTranslatorService _translatorService;
        private readonly IElasticClient _client;
        private readonly ILogger<ElasticSearchQueryHandler> _logger;
        private readonly ElasticSearchConfiguration _configuration;
        private readonly IEnumerable<IAggregationTranslationService> _collection;

        public ElasticSearchQueryHandler(IElasticSearchQueryTranslatorService translatorService, IElasticClient client,
            ILogger<ElasticSearchQueryHandler> logger, ElasticSearchConfiguration configuration,
            IEnumerable<IAggregationTranslationService> collection)
        {
            _translatorService = translatorService;
            _client = client;
            _logger = logger;
            _configuration = configuration;
            _collection = collection;
        }

        public async Task<IQueryResult> HandleAsync(SearchQuery query)
        {
            var indexName = _configuration.IndexPrefixName + query.Index.ToLowerInvariant();
            SearchDescriptor<ISearchModel> searchDescriptor = _translatorService.Translate(query);
            var index = await _client.Indices.ExistsAsync(indexName);
            if (!index.Exists)
            {
                return new SimplQueryResult()
                {
                    SearchModels = new List<ISearchModel>(),
                    Pagination = new Pagination()
                    {
                        Total = 0,
                        TotalNumberOfPages = 0
                    }
                };
            }

            var result =
                await _client.SearchAsync<ElasticSearchModel>(s =>
                    searchDescriptor.Index(query.Index.ToLowerInvariant()));
            if (_configuration.UseDebugStream)
            {
                _logger.LogInformation(result.DebugInformation);
                _logger.LogInformation(searchDescriptor.ToString());
            }

            var resultModel = new SimplQueryResult()
            {
                SearchModels = result.Documents.Select(ElasticSearchModelMapper.Map).ToList(),
                Aggregations = TranslateAggregations(result.Aggregations),
                Pagination = new Pagination()
                {
                    CurrentPage = query.Page,
                    PageSize = query.PageSize,
                    Total = result.Total,
                    TotalNumberOfPages = (int)Math.Ceiling((result.Total / (double)query.PageSize))
                },
            };
            return resultModel;
        }

        private List<IAggregation> TranslateAggregations(AggregateDictionary resultAggregations)
        {
            var list = new List<IAggregation>();

            foreach (var aggregation in resultAggregations)
            {
                var aggregationModel = TranslateAggregate(aggregation);
                list.Add(aggregationModel);
            }

            return list;
        }
        //todo: figure out if we can simplify stuff there
        private IAggregation TranslateAggregate(KeyValuePair<string, IAggregate> aggregation)
        {
            var type = aggregation.Value.GetType();
            var handlerType =
                typeof(IAggregationTranslationService<>).MakeGenericType(type);
            var translator =
                _collection.FirstOrDefault(x => x.GetType().GetInterfaces().Any(x => x == handlerType));
            var aggregationModel = translator.Translate(aggregation);
            aggregationModel.NestedAggregation=    TranslateSubAggregate(aggregation.Value);
            return aggregationModel;
        }

        private List<IAggregation> TranslateSubAggregate(IAggregate aggregationValue)
        {
            var list = new List<IAggregation>();
            //todo: add other options of buckets in future
            switch (aggregationValue)
            {
                case BucketAggregate keyedBucket:
                    list.AddRange(keyedBucket.Items.Select(x => x as KeyedBucket<object>).SelectMany(x => x.Select(y => TranslateAggregate(y))));
                    break;
                 case SingleBucketAggregate singleBucketAggregate:
                     list.AddRange(singleBucketAggregate.Select(y => TranslateAggregate(y)));
                     break;
            }

            return list;
        }
    }
}