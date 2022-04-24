using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SImpl.CQRS.Queries;
using SImpl.SearchModule.Abstraction.Fields;
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
            try
            {
                var query = new FluentApiSearchQueryCreator(new SearchQuery()
                    {
                        Index = "headlesssearchindex",
                        PageSize = request.PageSize,
                        Page = request.Page,
                    })
                    .CreateSearchQuery(s =>
                    {
                        s.CreateAggregationQuery(f =>
                        {
                           
                            f.CreateFilterQuery();
                            f.CreateTermQuery(x=>x.WithName().WithField())
                        })
                        s.CreateAggregationQuery<TermAggregation>(f =>
                            {
                                f.AggregationName = "facets";
                                f.TermFieldName = "facet";
                            })
                            .CreateAggregationQuery<SImpl.SearchModule.Abstraction.Queries.FilterAggregationQuery>(f =>
                            {
                                f.AggregationName = "filters";
                                if (request.Facets.Any())
                                {
                                    f.Queries.Add(Occurance.Must, new BoolSearchSubQuery()
                                    {
                                        Occurance = Occurance.Must, NestedQueries = new List<ISearchSubQuery>()
                                        {
                                            new TermsSubQuery()
                                            {
                                                Field = "facet",
                                                Value = request.Facets.Select(x => x.Key as object).ToList()
                                            }
                                        }
                                    });
                                    f.NestedAggregations = new List<IAggregationQuery>()
                                    {
                                        new TermAggregation()
                                        {
                                            AggregationName = "filters",
                                            TermFieldName = "tags"
                                        },
                                    };
                                }
                                else
                                {
                                    f.NestedAggregations = new List<IAggregationQuery>()
                                    {
                                        new TermAggregation()
                                        {
                                            AggregationName = "filters",
                                            TermFieldName = "tags"
                                        },
                                    };
                                }
                            })
                            .CreatePostFilterQuery(e =>
                                e.Must().CreateBoolQuery(facetQuery =>
                                {
                                    foreach (var facet in request.Facets)
                                    {
                                        facetQuery.Should()
                                            .CreateTermQuery(f =>
                                                f
                                                    .WithField("facet")
                                                    .WithValue(facet.Key));
                                    }
                                }))
                            .CreateBoolQuery(filterquery =>
                            {
                                filterquery.Must().CreateBoolQuery(subquery =>
                                    {
                                        foreach (var filter in request.Filters)
                                        {
                                            foreach (var option in filter.Options)
                                            {
                                                subquery.Should()
                                                    .CreateTermQuery(f =>
                                                        f
                                                            .WithField("tags")
                                                            .WithValue(option.OptionId));
                                            }
                                        }
                                    })
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
                                    .CreateBoolQuery(filterquery =>
                                    {
                                        filterquery.Must().CreateBoolQuery(subquery =>
                                        {
                                            foreach (var filter in request.PreFilters)
                                            {
                                                foreach (var option in filter.Options)
                                                {
                                                    subquery.Should()
                                                        .CreateTermQuery(f =>
                                                            f
                                                                .WithField("tags")
                                                                .WithValue(option.OptionId));
                                                }
                                            }
                                        });
                                    })
                                    /* //todo: we turn off that for now as we have issue with global repository
                                     .CreateTermQuery(t =>
                                    {
                                        if (request.SiteId != 0)
                                        {
                                            t
                                                .Must()
                                                .WithField("tags")
                                                .WithValue(request.SiteId);
                                        }
                                    })*/
                                    .CreateTermQuery(t =>
                                    {
                                        t
                                            .Must()
                                            .WithField("searchCulture")
                                            .WithValue(request.Culture.ToLowerInvariant().Replace("-", ""));
                                    })
                                    .CreatePrefixPhraseQuery(
                                        t =>
                                        {
                                            t
                                                .Must()
                                                .WithField("content")
                                                .WithValue(request.Term.ToLowerInvariant());
                                        });
                            });
                        //.CreateBoolQuery(e => e.Should().CreateBoolQuery());
                    });


                var result = await _queryDispatcher.QueryAsync(query) as SimplQueryResult;

                return result;
                ;
            }
            catch (Exception e)
            {
                var finalResult = new SimplQueryResult()
                {
                    IsSuccess = false,
                    ErrorMessage = "Search failed with internal error, please contact with Administrator"
                };

                return finalResult;
            }
        }

  
    }
}