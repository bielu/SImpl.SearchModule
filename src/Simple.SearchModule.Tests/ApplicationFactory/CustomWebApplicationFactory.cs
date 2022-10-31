using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SImpl.Runtime;
using SImpl.SearchModule.Core.Application.Services;
using SImpl.SearchModule.ElasticSearch.Configuration;

namespace Simple.SearchModule.Tests.ApplicationFactory
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webHost =>
                {
                    webHost.UseStartup<TStartup>();
                })
                .SImplify(simpl =>
                {
                    simpl.UseElastic(configure =>
                    {
                        var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                        configure.AuthenticationMode = AuthenticationModes.CloudAuthentication;
                        configure.UseDebugStream = true;

                        configure.SearchService = typeof(DirectIndexingService);
                        //  configure.UseZeroDowntimeIndexing = true; //todo: restore when fixed
                    });
                });
         
            return host;
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((hostingContext, configurationBuilder) =>
            {
                var type = typeof(TStartup);
                var path = @"C:\\OriginalApplication";
    
                configurationBuilder.AddJsonFile($"{path}\\appsettings.json", optional: true, reloadOnChange: true);
                configurationBuilder.AddEnvironmentVariables();
            });
    
            // if you want to override Physical database with in-memory database
            builder.ConfigureServices(services =>
            {
             
            });
        }
    }
}