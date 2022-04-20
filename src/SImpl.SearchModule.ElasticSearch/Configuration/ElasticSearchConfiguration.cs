using System;
using System.Collections;
using System.Collections.Generic;
using Elasticsearch.Net;
using Nest;
using SImpl.SearchModule.ElasticSearch.Application.Services;
using SImpl.SearchModule.ElasticSearch.Models;

namespace SImpl.SearchModule.ElasticSearch.Configuration
{
    public class ElasticSearchConfiguration
    {
        public AuthenticationModes AuthenticationMode { get; set; }
        public string CloudId { get; set; }
        public BasicAuthenticationCredentials BasicAuthentication { get; set; }

        public ApiKeyAuthenticationCredentials ApiAuthentication { get; set; }
        public BasicAuthenticationCredentials ConnectionSettingValues { get; set; }
        public bool UseDebugStream { get; set; } = false;
        public Uri Uri { get; set; }
        public string IndexPrefixName { get; set; } = "";
        public bool UseZeroDowntimeIndexing { get; set; }
        public Type ElasticSearchConnectionFactory { get; set; } = typeof(ElasticSearchConnectionFactory);

        public Type ElasticModelMapper { get; set; } = typeof(DefaultElasticMapper);

        public Dictionary<AnalyzerType, List<IElasticProperty>> ElasticPropertiesFields { get; set; } =
            new Dictionary<AnalyzerType, List<IElasticProperty>>()
            {
                {
                    AnalyzerType.Keyword, new List<IElasticProperty>()
                    {
                        new TextElasticProperty()
                        {
                            Name = "id"
                        },
                        new TextElasticProperty()
                        {
                            Name = "contentType"
                        },
                        new TextElasticProperty()
                        {
                            Name = "facet"
                        },
                        new TextElasticProperty()
                        {
                            Name = "tags"
                        },
                    }
                }
            };

        public ElasticSearchConfiguration MapProperty(AnalyzerType type, IElasticProperty property)
        {
            if (ElasticPropertiesFields.ContainsKey(type))
            {
                ElasticPropertiesFields[type].Add(property);
            }
            else
            {
                ElasticPropertiesFields.Add(type, new List<IElasticProperty>()
                {
                    property
                });
            }

            return this;
        }
    }
}