using System;
using SImpl.Host.Builders;

namespace SImpl.SearchModule.ElasticSearch.Configuration
{
    public static class ElasticSearchModuleConfigExtensions
    {
        public static void UseElastic(this ISImplHostBuilder hostBuilder, Action<ElasticSearchConfiguration> configure)
        {
            var existingModule = hostBuilder.GetConfiguredModule<ElasticSearchModule>();
            var config =  existingModule != null?  existingModule.Config : new ElasticSearchConfiguration();
            configure.Invoke(config);
            if (existingModule == null)
            {
                AttachModule(hostBuilder, config);
            }
        }

        private static void AttachModule(ISImplHostBuilder novicellAppBuilder,
            ElasticSearchConfiguration elasticSearchConfiguration)
        {
            var module = new ElasticSearchModule(elasticSearchConfiguration);

            novicellAppBuilder.AttachNewOrGetConfiguredModule<ElasticSearchModule>(() => module);
        }
    }
}