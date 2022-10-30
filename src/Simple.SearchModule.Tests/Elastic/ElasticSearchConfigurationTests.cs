using System.Threading.Tasks;
using SImpl.SearchModule.Core.Application.Services;
using SImpl.SearchModule.ElasticSearch.Configuration;
using Xunit;

namespace Simple.SearchModule.Tests.Elastic
{
    public class ElasticSearchConfigurationTests
    {
        public ElasticSearchConfigurationTests()
        {
            fluentApi = new ElasticSearchConfiguration();
        }

        public ElasticSearchConfiguration fluentApi { get; set; }

        [Fact]
        public async Task DebugStreamTurnoffByDefault()
        {
            Assert.Equal(false, fluentApi.UseDebugStream);
        }

        [Fact]
        public async Task CanSetupDebugStream()
        {
            fluentApi.UseDebugStream = true;
            Assert.Equal(true, fluentApi.UseDebugStream);
        }

        [Fact]
        public async Task CanSetupDirectIndexService()
        {
            fluentApi.SearchService = typeof(DirectIndexingService);
            Assert.Equal(typeof(DirectIndexingService), fluentApi.SearchService);
        }

        [Fact]
        public async Task DefaultIsCommandIndexService()
        {
            Assert.Equal(typeof(IndexingService), fluentApi.SearchService);
        }
    }
}