using System;
using SImpl.Host.Builders;

namespace SImpl.SearchModule.Examine.Configuration
{
    public static class ElasticSearchModuleConfigExtensions
    {
        public static void UseExamine(this ISImplHostBuilder hostBuilder, Action<ExamineSearchConfiguration> configure)
        {
            var existingModule = hostBuilder.GetConfiguredModule<ExamineSearchModule>();
            var config =  existingModule != null?  existingModule.Config : new ExamineSearchConfiguration();
            configure.Invoke(config);
            if (existingModule == null)
            {
                AttachModule(hostBuilder, config);
            }
        }

        private static void AttachModule(ISImplHostBuilder novicellAppBuilder,
            ExamineSearchConfiguration examineSearchConfiguration)
        {
            var module = new ExamineSearchModule(examineSearchConfiguration);

            novicellAppBuilder.AttachNewOrGetConfiguredModule<ExamineSearchModule>(() => module);
        }
    }
}