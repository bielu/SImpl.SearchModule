using System;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nest;
using SImpl.SearchModule.ElasticSearch.Configuration;

namespace SImpl.SearchModule.ElasticSearch.Application.Services
{
    public interface IElasticSearchConnectionFactory
    {
        ConnectionSettings CreateConnection();
    }

    public class ElasticSearchConnectionFactory : IElasticSearchConnectionFactory
    {
        private readonly ElasticSearchConfiguration _configuration;
        private readonly IConfiguration _appSettings;
        private readonly ILogger<ElasticSearchConnectionFactory> _logger;
        private  ConnectionSettings _connectionString;
        public ElasticSearchConnectionFactory(ElasticSearchConfiguration configuration, IConfiguration appSettings,
            ILogger<ElasticSearchConnectionFactory> logger)
        {
            _configuration = configuration;
            _appSettings = appSettings;
            _logger = logger;
        }

        public ConnectionSettings CreateConnection()
        {
            if (_connectionString != null)
            {
                return _connectionString;
            }
            ConnectionSettings connectionString;
            SingleNodeConnectionPool pool;
            switch (_configuration.AuthenticationMode)
            {
                default:
                case AuthenticationModes.Default:
                    pool = new SingleNodeConnectionPool(new Uri("http://localhost:9200"));
                    connectionString = new ConnectionSettings(pool);
                    break;

                case AuthenticationModes.Uri:
                    if (_configuration.Uri == null)
                    {
                        var elasticAuth = _appSettings["Simpl:SearchModule:Elastic:url"];
                        _configuration.Uri = new Uri(elasticAuth);
                    }
                    pool = new SingleNodeConnectionPool(_configuration.Uri);
                    connectionString = new ConnectionSettings(pool);
                    break;
                case AuthenticationModes.CloudAuthentication:
                    if (string.IsNullOrEmpty(_configuration.CloudId))
                    {
                        _configuration.CloudId = _appSettings["Simpl:SearchModule:Elastic:cloudId"];
                        _configuration.BasicAuthentication = new BasicAuthenticationCredentials(
                            _appSettings["Simpl:SearchModule:Elastic:userName"],
                            _appSettings["Simpl:SearchModule:Elastic:password"]);
                    }

                    pool = new CloudConnectionPool(_configuration.CloudId, _configuration.BasicAuthentication);
                    connectionString =
                        new ConnectionSettings(pool);
                    break;
                case AuthenticationModes.CloudApiAuthentication:
                    pool = new CloudConnectionPool(_configuration.CloudId, _configuration.ApiAuthentication);

                    connectionString =
                        new ConnectionSettings(pool);
                    break;
                case AuthenticationModes.ConnectionSettingsValues:
                    pool = new CloudConnectionPool(_configuration.CloudId, _configuration.BasicAuthentication);
                    connectionString =
                        new ConnectionSettings(pool);
                    break;
            }

            if (_configuration.UseDebugStream)
            {
                connectionString.EnableDebugMode();
            }

            _connectionString = connectionString;
            return _connectionString;
        }
    }
}