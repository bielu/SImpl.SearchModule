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
using SImpl.SearchModule.ElasticSearch.Application.Services.MultiSearch;
using SImpl.SearchModule.ElasticSearch.Application.Services.Result;
using SImpl.SearchModule.ElasticSearch.Configuration;
using SImpl.SearchModule.ElasticSearch.Models;
using IAggregation = SImpl.SearchModule.Abstraction.Models.IAggregation;
using IBucket = Nest.IBucket;

namespace SImpl.SearchModule.ElasticSearch.Application.QueryHandlers
{
    public class ElasticSearchMultiSearchQueryHandler : IQueryHandler<MultiSearchQuery, IMultiSearchQueryResult>
    {
        private IElasticSearchMultiQueryTranslatorService _translatorService;
        private readonly IElasticClient _client;
        private readonly ILogger<ElasticSearchMultiSearchQueryHandler> _logger;
        private readonly ElasticSearchConfiguration _configuration;
        private readonly IEnumerable<IAggregationTranslationService> _collection;

        public ElasticSearchMultiSearchQueryHandler(IElasticSearchMultiQueryTranslatorService translatorService,
            IElasticClient client,
            ILogger<ElasticSearchMultiSearchQueryHandler> logger, ElasticSearchConfiguration configuration,
            IEnumerable<IAggregationTranslationService> collection)
        {
            _translatorService = translatorService;
            _client = client;
            _logger = logger;
            _configuration = configuration;
            _collection = collection;
        }

        public async Task<IMultiSearchQueryResult> HandleAsync(MultiSearchQuery query)
        {
            MultiSearchDescriptor searchDescriptor = _translatorService.Translate(query);

            MultiSearchResponse result =
                await _client.MultiSearchAsync(searchDescriptor);
            if (_configuration.UseDebugStream)
            {
                _logger.LogInformation(result.DebugInformation);
                _logger.LogInformation(searchDescriptor.ToString());
            }

            var resultModel = new MultiSearchQueryResult()
            {
                Results = new Dictionary<string, IQueryResult>()
            };
            foreach (var singularQuery in query.Queries)
            {
                var searchResponse = result.GetResponse<ElasticSearchModel>(singularQuery.Key);
                if (searchResponse.IsValid)
                {
                    resultModel.Results.Add(singularQuery.Key, new SimplQueryResult()
                    {
                        SearchModels = searchResponse.Documents.Select(ElasticSearchModelMapper.Map).ToList(),
                        Aggregations = TranslateAggregations(searchResponse.Aggregations),
                        Pagination = new Pagination()
                        {
                            CurrentPage = singularQuery.Value.Page,
                            PageSize = singularQuery.Value.PageSize,
                            Total = searchResponse.Total,
                            TotalNumberOfPages =
                                (int)Math.Ceiling((searchResponse.Total / (double)singularQuery.Value.PageSize))
                        },
                        HighLighter = TranslateHighLighter(searchResponse.Hits)
                    });
                }
            }

            return resultModel;
        }


        private HighLighter TranslateHighLighter(IReadOnlyCollection<IHit<ElasticSearchModel>> resultHits)
        {
            var highLghter = new HighLighter();
            foreach (var hit in resultHits)
            {
                foreach (var highlightHit in hit.Highlight)
                {
                    highLghter.Add(highlightHit.Key, highlightHit.Value.Select(x => x as object).ToList());
                }
            }

            return highLghter;
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
            aggregationModel.NestedAggregation = TranslateSubAggregate(aggregation.Value);
            return aggregationModel;
        }

        private List<IAggregation> TranslateSubAggregate(IAggregate aggregationValue)
        {
            var list = new List<IAggregation>();
            //todo: add other options of buckets in future
            switch (aggregationValue)
            {
                case BucketAggregate keyedBucket:
                    list.AddRange(keyedBucket.Items.Select(x => x as KeyedBucket<object>)
                        .SelectMany(x => x.Select(y => TranslateAggregate(y))));
                    break;
                case SingleBucketAggregate singleBucketAggregate:
                    list.AddRange(singleBucketAggregate.Select(y => TranslateAggregate(y)));
                    break;
            }

            return list;
        }
    }
}