using System.Reflection;
using Algolia.Search.Clients;
using Microsoft.Extensions.DependencyInjection;
using SImpl.CQRS.Commands;
using SImpl.CQRS.Queries;
using SImpl.Host.Builders;
using SImpl.Modules;
using SImpl.SearchModule.Algolia.Application.Factories;
using SImpl.SearchModule.Algolia.Application.LuceneEngine;
using SImpl.SearchModule.Algolia.Application.Services;
using SImpl.SearchModule.Algolia.Application.Services.SubQueries;
using SImpl.SearchModule.Algolia.Configuration;

namespace SImpl.SearchModule.Algolia
{
    
    public class ExamineSearchModule : IAspNetPreModule,IHostBuilderConfigureModule, ISImplModule
    {
        public ExamineSearchModule(AlgoliaSearchConfiguration config)
        {
            Config = config;
        }

        public AlgoliaSearchConfiguration Config { get; set; }

        public string Name { get; } = nameof(ExamineSearchModule);
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
        }

        public void ConfigureHostBuilder(ISImplHostBuilder hostBuilder)
        {
            hostBuilder.UseCqrsCommands(x => x.AddCommandHandlersFromAssembly(typeof(ExamineSearchModule).Assembly));
            hostBuilder.UseCqrsQueries(x => x.AddQueryHandlersFromAssembly(typeof(ExamineSearchModule).Assembly));
          

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(AlgoliaSearchConfiguration), (services) => Config);
            services.AddSingleton<ISearchClient>(new SearchClient(Config.AppId, Config.ApiKey));
            services.AddTransient<IAlgoliaQueryTranslatorService, BaseAlgoliaQueryTranslatorService>();
            services.AddTransient(typeof(IIndexingService),
                Config.SearchService);
            
            services.AddSingleton<ConfigurationEnabledDirectoryFactory>();
            services.Scan(s =>
                s.FromAssemblies(new List<Assembly>(){typeof(ExamineSearchModule).Assembly})
                    .AddClasses(c => c.AssignableTo(typeof(IAlgoliaQueryTranslatorService)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
            services.Scan(s =>
                s.FromAssemblies(new List<Assembly>(){typeof(ExamineSearchModule).Assembly})
                    .AddClasses(c => c.AssignableTo(typeof(ISubQueryExamineTranslator<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
            foreach (var indexName in Config.IndexName)
            {
                services.AddExamineLuceneIndex<SimplExamineLuceneIndex, ConfigurationEnabledDirectoryFactory>(Config.IndexPrefixName+indexName, Config.FieldsDefinition);

            }
        }

    }
}