using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SImpl.SearchModule.Abstraction.Queries.subqueries;
using SImpl.SearchModule.ElasticSearch.Application.Services.SubQueries;
using Xunit;

namespace Simple.SearchModule.Tests.Search.Query
{
    public class SubQueryTranslationTests
    {
        [Fact]
        public async Task CanDateRangeChecksIfDaterangeIsCorrect()
        {
            try
            {
                DateRangeQuery query = new DateRangeQuery()
                {
                    Field = "dateRange",
                    BoostValue = 1,
                    IncludeMaxEdge = true,
                    IncludeMinEdge = false,
                    MaxValue = new DateTime(),
                    MinValue = DateTime.MinValue
                };
                DateRangeQueryElasticTranslator translator = new DateRangeQueryElasticTranslator();
                var elasticQuery = translator.Translate<DateRangeQuery>(new List<ISubQueryElasticTranslator>(), query);
            }
            catch (ArgumentException e)
            {
                Assert.Equal(new ArgumentException("Max value has to be higher than minimal value").Message, e.Message);
            }
    }
        [Fact]
        public async Task CanTranslateDateRangeQueryToElastic()
        {
            DateRangeQuery query = new DateRangeQuery()
            {
                Field = "dateRange",
                BoostValue = 1,
                IncludeMaxEdge = true,
                IncludeMinEdge = false,
                MaxValue =DateTime.MaxValue,
                MinValue = DateTime.MinValue
            };
            DateRangeQueryElasticTranslator translator = new DateRangeQueryElasticTranslator();
            var elasticQuery = translator.Translate<DateRangeQuery>(new List<ISubQueryElasticTranslator>(),query);
            
        }
        

        
    }
}