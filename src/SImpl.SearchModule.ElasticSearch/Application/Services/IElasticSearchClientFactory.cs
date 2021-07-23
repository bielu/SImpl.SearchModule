using Nest;
using SImpl.SearchModule.ElasticSearch.Configuration;

namespace SImpl.SearchModule.ElasticSearch.Application.Services
{
    public interface IElasticSearchClientFactory
    {
        IElasticClient CreateClient();
    }
    public class  ElasticSearchClientFactory: IElasticSearchClientFactory
    {
        private ElasticSearchConfiguration _configuration;

        public ElasticSearchClientFactory(ElasticSearchConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IElasticClient CreateClient()
        {
            
            switch (_configuration.AuthenticationMode)
            {
                default:
                case  AuthenticationModes.Default:
                    return new ElasticClient();
                    break;
                case  AuthenticationModes.Uri:
                    return new ElasticClient(_configuration.Uri);
                    break;
                case AuthenticationModes.CloudAuthentication:
                    return new ElasticClient(_configuration.CloudId, _configuration.BasicAuthentication);
                    break;
                case AuthenticationModes.CloudApiAuthentication:
                    return new ElasticClient(_configuration.CloudId, _configuration.BasicAuthentication);
                    break;
                case AuthenticationModes.ConnectionSettingsValues:
                    return new ElasticClient(_configuration.CloudId, _configuration.ConnectionSettingValues);
                    break;
            }
            
        }
    }
}