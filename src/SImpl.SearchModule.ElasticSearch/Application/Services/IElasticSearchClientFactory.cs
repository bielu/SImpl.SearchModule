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

        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
        };
        public ElasticSearchClientFactory(ElasticSearchConfiguration configuration, IConfiguration appSettings, ILogger<ElasticSearchClientFactory> logger)
        {
            _configuration = configuration;
            _appSettings = appSettings;
            _logger = logger;
        }

        public IElasticClient CreateClient()
        {
            ConnectionSettings connectionString;
            SingleNodeConnectionPool pool;
            switch (_configuration.AuthenticationMode)
            {
                default:
                case AuthenticationModes.Default:
                    pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
                    connectionString = new ConnectionSettings(pool);
                    if (_configuration.UseDebugStream)
                    {
                        connectionString.EnableDebugMode();
                    }

                    return new ElasticClient(connectionString);
                    break;
                case AuthenticationModes.Uri: 
                    pool = new SingleNodeConnectionPool(_configuration.Uri);

                    connectionString = new ConnectionSettings(pool);

                    if (_configuration.UseDebugStream)
                    {
                        connectionString.EnableDebugMode();
                    }

                    return new ElasticClient(connectionString);
                    break;
                case AuthenticationModes.CloudAuthentication:
                    if (string.IsNullOrEmpty(_configuration.CloudId))
                    {
                        _configuration.CloudId = _appSettings["Simpl:SearchModule:Elastic:cloudId"];
                        _configuration.BasicAuthentication = new BasicAuthenticationCredentials(_appSettings["Simpl:SearchModule:Elastic:userName"],_appSettings["Simpl:SearchModule:Elastic:password"]);
                    }
                    pool = new CloudConnectionPool(_configuration.CloudId, _configuration.BasicAuthentication);
                    connectionString =
                        new ConnectionSettings(pool);
                    
                    if (_configuration.UseDebugStream)
                    {
                        connectionString.EnableDebugMode();
                    }
                    return new ElasticClient(connectionString);
                    break;
                case AuthenticationModes.CloudApiAuthentication:
                    pool = new CloudConnectionPool(_configuration.CloudId, _configuration.ApiAuthentication);

                    connectionString =
                        new ConnectionSettings(pool);

                    if (_configuration.UseDebugStream)
                    {
                        connectionString.EnableDebugMode();
                    }

                    return new ElasticClient(connectionString);
                    break;
                case AuthenticationModes.ConnectionSettingsValues:
                    pool = new CloudConnectionPool(_configuration.CloudId, _configuration.BasicAuthentication);

                    connectionString =
                        new ConnectionSettings(pool);

                    if (_configuration.UseDebugStream)
                    {
                        connectionString.EnableDebugMode();
                    }

                    return new ElasticClient(connectionString);
                    break;
            }
        }
    }
}