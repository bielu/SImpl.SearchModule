using System.Collections.Generic;
using System.Reflection;
using Examine;
using Examine.Lucene.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SImpl.CQRS.Commands;
using SImpl.CQRS.Queries;
using SImpl.Host.Builders;
using SImpl.Hosts.WebHost.Modules;
using SImpl.Modules;
using SImpl.SearchModule.Core.Application.Services;
using SImpl.SearchModule.Examine.Application.Factoriesz;
using SImpl.SearchModule.Examine.Application.LuceneEngine;
using SImpl.SearchModule.Examine.Application.Services;
using SImpl.SearchModule.Examine.Application.Services.SubQueries;
using SImpl.SearchModule.Examine.Configuration;

namespace SImpl.SearchModule.Examine
{
    
    public class ExamineSearchModule : IAspNetPreModule,IHostBuilderConfigureModule, ISImplModule
    {
        public ExamineSearchModule(ExamineSearchConfiguration config)
        {
            Config = config;
        }

        public ExamineSearchConfiguration Config { get; set; }

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
            services.AddSingleton(typeof(ExamineSearchConfiguration), (services) => Config);
            services.AddTransient<IExamineQueryTranslatorService, BaseExamineQueryTranslatorService>();
            services.AddTransient<IIndexingService, IndexingService>();
            services.AddSingleton<ConfigurationEnabledDirectoryFactory>();
            services.Scan(s =>
                s.FromAssemblies(new List<Assembly>(){typeof(ExamineSearchModule).Assembly})
                    .AddClasses(c => c.AssignableTo(typeof(IExamineQueryTranslatorService)))
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