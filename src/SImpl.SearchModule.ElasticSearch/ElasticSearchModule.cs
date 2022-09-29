using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nest;
using SImpl.CQRS.Commands;
using SImpl.CQRS.Queries;
using SImpl.Host.Builders;
using SImpl.Hosts.WebHost.Modules;
using SImpl.Modules;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Core.Application.Services;
using SImpl.SearchModule.ElasticSearch.Application.Services;
using SImpl.SearchModule.ElasticSearch.Application.Services.Result;
using SImpl.SearchModule.ElasticSearch.Application.Services.SubQueries;
using SImpl.SearchModule.ElasticSearch.Configuration;

namespace SImpl.SearchModule.ElasticSearch
{
    public class ElasticSearchModule : IAspNetPreModule,IHostBuilderConfigureModule, ISImplModule
    {
        public ElasticSearchModule(ElasticSearchConfiguration config)
        {
            Config = config;
        }

        public ElasticSearchConfiguration Config { get; set; }

        public string Name { get; } = nameof(ElasticSearchModule);
        public void ConfigureHostBuilder(ISImplHostBuilder hostBuilder)
        {
            hostBuilder.UseCqrsCommands(x => x.AddCommandHandlersFromAssembly(typeof(ElasticSearchModule).Assembly));
            hostBuilder.UseCqrsQueries(x => x.AddQueryHandlersFromAssembly(typeof(ElasticSearchModule).Assembly));

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(ElasticSearchConfiguration), (services) => Config);
            services.AddSingleton(typeof(IElasticSearchConnectionFactory), Config.ElasticSearchConnectionFactory);

            services.AddSingleton(typeof(IElasticMapper), Config.ElasticModelMapper);
            services.AddTransient(typeof(IElasticSearchClientFactory), typeof(ElasticSearchClientFactory));
            services.AddTransient(typeof(IElasticClient), provider => provider.GetService<IElasticSearchClientFactory>().CreateClient());
            services.AddTransient<IElasticSearchQueryTranslatorService, BaseElasticSearchQueryTranslatorService>();
            services.AddTransient(typeof(IIndexingService),
                Config.IndexingService);
            services.Scan(s =>
                s.FromAssemblies(new List<Assembly>(){typeof(ElasticSearchModule).Assembly})
                    .AddClasses(c => c.AssignableTo(typeof(ISubQueryElasticTranslator<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
            services.Scan(s =>
                s.FromAssemblies(new List<Assembly>(){typeof(ElasticSearchModule).Assembly})
                    .AddClasses(c => c.AssignableTo(typeof(IFacetElasticTranslator<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
            services.Scan(s =>
                s.FromAssemblies(new List<Assembly>(){typeof(ElasticSearchModule).Assembly})
                    .AddClasses(c => c.AssignableTo(typeof(IAggregationTranslationService<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        }
    }
}