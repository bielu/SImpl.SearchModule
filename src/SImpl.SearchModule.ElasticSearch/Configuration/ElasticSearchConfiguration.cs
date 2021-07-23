using System;
using Elasticsearch.Net;

namespace SImpl.SearchModule.ElasticSearch.Configuration
{
    public class ElasticSearchConfiguration
    {
        public AuthenticationModes AuthenticationMode { get; set; }
        public string CloudId { get; set; }
        public BasicAuthenticationCredentials BasicAuthentication { get; set; }
        
        public ApiKeyAuthenticationCredentials ApiAuthentication { get; set; }
        public BasicAuthenticationCredentials ConnectionSettingValues { get; set; }
        public Uri Uri { get; set; }
    }
}