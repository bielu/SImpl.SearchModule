using System.Linq;
using System.Threading.Tasks;
using SImpl.SearchModule.Core.Application.Services;
using SImpl.SearchModule.ElasticSearch.Configuration;
using SImpl.SearchModule.ElasticSearch.Models;
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
        [Fact]
        public async Task CanSetupCustomKeywordPropertyMapping()
        {
            string propertyName = "myproperty";
            fluentApi.MapProperty(AnalyzerType.Keyword, new TextElasticProperty()
            {
                Name = propertyName
            });
            Assert.Equal(true, fluentApi.ElasticPropertiesFields.ContainsKey(AnalyzerType.Keyword));

            var keywordProperties = fluentApi.ElasticPropertiesFields[AnalyzerType.Keyword];
            Assert.Equal(true, keywordProperties.Any(x=>x.Name == propertyName));
        }
    }
}