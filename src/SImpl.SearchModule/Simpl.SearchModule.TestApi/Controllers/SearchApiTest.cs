using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SImpl.SearchModule.FluentApi.Configuration;
using SImpl.SearchModule.FluentApi.Configuration.Fluent.Extensions;

namespace Simpl.SearchModule.TestApi.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Components.Route("[controller]")]
    public class SearchApiTestController : ControllerBase
    {
        private readonly ILogger<SearchApiTestController> _logger;

        private readonly IQueryDispatcher _queryDispatcher;
        public  SearchApiTestController(ILogger<SearchApiTestController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var query = new FluentApiSearchQueryCreator();
            query.CreateSearchQuery(e=>
            {
                e.CreateBoolQuery(e => e.Must()
                        .CreateTermQuery(
                            t=>t
                                .Should()
                                .WithField("Title")
                                .WithValue("I am the best")
                            )
                    )
                    
                    .CreateBoolQuery(e => e.Should().CreateBoolQuery());
                
            });
            return await _queryDispatcher.QueryAsync(query);
          
        }
    }
}