using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using SImpl.Runtime;
using SImpl.SearchModule.Core.Application.Services;
using SImpl.SearchModule.ElasticSearch.Configuration;
using Xunit;

namespace Simple.SearchModule.Tests.Elastic
{
    public class ElasticSearchHostBuilderTests
    {
        [Fact]
        public async Task ShouldStartServer()
        {
            // Build your "app"
             await Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webHost =>
                {
                    webHost.UseStartup<StartupElastic>();
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
                }).Build().StartAsync();
            
        }
    }
}