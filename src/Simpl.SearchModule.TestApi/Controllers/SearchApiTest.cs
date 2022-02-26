using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SImpl.CQRS.Queries;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;
using SImpl.SearchModule.Abstraction.Results;
using SImpl.SearchModule.FluentApi.Configuration;
using SImpl.SearchModule.FluentApi.Configuration.Fluent.Extensions;
using Simpl.SearchModule.TestApi.Models;

namespace Simpl.SearchModule.TestApi.Controllers
{
    [ApiController]
    [Microsoft.AspNetCore.Components.Route("[controller]")]
    public class SearchApiTestController : ControllerBase
    {
        private readonly ILogger<SearchApiTestController> _logger;

        private readonly IQueryDispatcher _queryDispatcher;
        public  SearchApiTestController(ILogger<SearchApiTestController> logger, IQueryDispatcher queryDispatcher)
        {
            _logger = logger;
            _queryDispatcher = queryDispatcher;
        }
        
        [HttpGet]
        public async Task<SimplQueryResult> Get(SearchRequest request)
        {
           var query = new FluentApiSearchQueryCreator(new SearchQuery()
                    {
                        Index = "headlesssearchindex",
                        PageSize = request.PageSize,
                        Page = request.Page
                    })
                    .CreateSearchQuery(s =>
                        {
                            s.CreateAggregationQuery<TermAggregation>(f =>
                                {
                                    f.AggregationName = "facets";
                                    f.TermFieldName = "facet";
                                }).CreateBoolQuery(e => e.Must()
                                        .CreateBoolQuery(facetQuery =>
                                        {
                                            foreach (var facet in request.ContentTypes)
                                            {
                                                facetQuery.Should()
                                                    .CreateTermQuery(f =>
                                                        f
                                                            .WithField("contentType")
                                                            .WithValue(facet));
                                            }
                                        })
                                       
                                        .CreateTermQuery(t =>
                                        {
                                            t
                                                .Must()
                                                .WithField("searchCulture")
                                                .WithValue(request.Culture.ToLowerInvariant().Replace("-", ""));
                                        })
                                        .CreatePrefixQuery(
                                            t =>
                                            {
                                                t
                                                    .Must()
                                                    .WithField("content")
                                                    .WithValue(request.Term.ToLowerInvariant());
                                            })
                                    );
                                    //.CreateBoolQuery(e => e.Should().CreateBoolQuery());
                                });


                            var result = await _queryDispatcher.QueryAsync(query) as SimplQueryResult;
                            return result;


        }
    }
}