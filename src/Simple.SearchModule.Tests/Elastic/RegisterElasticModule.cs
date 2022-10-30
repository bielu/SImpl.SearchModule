using Microsoft.Extensions.DependencyInjection;
using SImpl.SearchModule.ElasticSearch.Configuration;
using Simple.SearchModule.Tests.ApplicationFactory;
using Xunit;

namespace Simple.SearchModule.Tests.Elastic
{
    public class RegisterElasticModuleTest : IClassFixture<CustomWebApplicationFactory<StartupElastic>>
    {
        public CustomWebApplicationFactory<StartupElastic> _factory;
        public RegisterElasticModuleTest(CustomWebApplicationFactory<StartupElastic> factory)
        {
            _factory = factory;
            _factory.CreateClient();
        }

        [Fact]
        public void CanBoot()
        {
            Assert.Equal(true, true);
        }
        [Fact]
        public void CanRetrieveConfiguration()
        {
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                var configuration = scope.ServiceProvider.GetRequiredService<ElasticSearchConfiguration>();
                
                Assert.NotNull(configuration);
            }
        }
    }
}