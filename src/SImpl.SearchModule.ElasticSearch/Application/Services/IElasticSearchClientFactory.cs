using System;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json;
using SImpl.SearchModule.ElasticSearch.Configuration;

namespace SImpl.SearchModule.ElasticSearch.Application.Services
{
    public interface IElasticSearchClientFactory
    {
        IElasticClient CreateClient();
    }

    public class ElasticSearchClientFactory : IElasticSearchClientFactory
    {
        private ElasticSearchConfiguration _configuration;
        private readonly IConfiguration _appSettings;
        private readonly ILogger<ElasticSearchClientFactory> _logger;
        private readonly IElasticSearchConnectionFactory _elasticSearchConnectionFactory;

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
        };
        public ElasticSearchClientFactory(ILogger<ElasticSearchClientFactory> logger, IElasticSearchConnectionFactory elasticSearchConnectionFactory)
        {
            _logger = logger;
            _elasticSearchConnectionFactory = elasticSearchConnectionFactory;
        }

        public IElasticClient CreateClient()
        {
           

                    return new ElasticClient(_elasticSearchConnectionFactory.CreateConnection());
            
        }
    }
}