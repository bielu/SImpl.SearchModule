using System;
using Elasticsearch.Net;
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
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
        };
        public ElasticSearchClientFactory(ElasticSearchConfiguration configuration)
        {
            _configuration = configuration;
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
                    connectionString = new ConnectionSettings(pool, (builtin, settings) =>new NewtonsoftSerializer(JsonSerializerSettings));
                    if (_configuration.UseDebugStream)
                    {
                        connectionString.DisableDirectStreaming();
                    }

                    return new ElasticClient(connectionString);
                    break;
                case AuthenticationModes.Uri: 
                    pool = new SingleNodeConnectionPool(_configuration.Uri);

                    connectionString = new ConnectionSettings(pool, (builtin, settings) =>new NewtonsoftSerializer(JsonSerializerSettings));

                    if (_configuration.UseDebugStream)
                    {
                        connectionString.DisableDirectStreaming();
                    }

                    return new ElasticClient(connectionString);
                    break;
                case AuthenticationModes.CloudAuthentication:
                    pool = new CloudConnectionPool(_configuration.CloudId, _configuration.BasicAuthentication);
                    connectionString =
                        new ConnectionSettings(pool, (builtin, settings) =>new NewtonsoftSerializer(JsonSerializerSettings));
                    
                    if (_configuration.UseDebugStream)
                    {
                        connectionString.DisableDirectStreaming();
                    }
                    return new ElasticClient(connectionString);
                    break;
                case AuthenticationModes.CloudApiAuthentication:
                    pool = new CloudConnectionPool(_configuration.CloudId, _configuration.ApiAuthentication);

                    connectionString =
                        new ConnectionSettings(pool, (builtin, settings) =>new NewtonsoftSerializer(JsonSerializerSettings));

                    if (_configuration.UseDebugStream)
                    {
                        connectionString.DisableDirectStreaming();
                    }

                    return new ElasticClient(connectionString);
                    break;
                case AuthenticationModes.ConnectionSettingsValues:
                    pool = new CloudConnectionPool(_configuration.CloudId, _configuration.BasicAuthentication);

                    connectionString =
                        new ConnectionSettings(pool,(builtin, settings) =>new NewtonsoftSerializer(JsonSerializerSettings));

                    if (_configuration.UseDebugStream)
                    {
                        connectionString.DisableDirectStreaming();
                    }

                    return new ElasticClient(connectionString);
                    break;
            }
        }
    }
}