using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Examine;
using Microsoft.Extensions.Logging;
using SImpl.CQRS.Queries;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Results;
using SImpl.SearchModule.Examine.Application.LuceneEngine;
using SImpl.SearchModule.Examine.Application.Services;
using SImpl.SearchModule.Examine.Configuration;
using SImpl.SearchModule.Examine.Models;
using IAggregation = SImpl.SearchModule.Abstraction.Models.IAggregation;
using IQuery = Examine.Search.IQuery;

namespace SImpl.SearchModule.Examine.Application.QueryHandlers
{
    public class ExamineSearchQueryHandler : IQueryHandler<SearchQuery, IQueryResult>
    {
        private readonly IExamineManager _examineManager;
        private readonly ExamineSearchConfiguration _configuration;
        private readonly ILogger<ExamineSearchQueryHandler> _logger;
        private readonly IExamineQueryTranslatorService _examineQueryTranslatorService;


        public ExamineSearchQueryHandler(IExamineManager examineManager, ExamineSearchConfiguration configuration,
            ILogger<ExamineSearchQueryHandler> logger, IExamineQueryTranslatorService examineQueryTranslatorService)
        {
            _examineManager = examineManager;
            _configuration = configuration;
            _logger = logger;
            _examineQueryTranslatorService = examineQueryTranslatorService;
        }

        public async Task<IQueryResult> HandleAsync(SearchQuery query)
        {
            var indexName = _configuration.IndexPrefixName + query.Index.ToLowerInvariant();
            _examineManager.TryGetIndex(indexName,
                out IIndex examineIndex);
            if (examineIndex == null)
            {
                _logger.LogError($"Examine index not found {indexName}");
                return new SimplQueryResult()
                {
                    SearchModels = new List<ISearchModel>(),
                    Pagination = new Pagination()
                    {
                        Total = 0,
                        TotalNumberOfPages = 0
                    },
                    HighLighter = new HighLighter()
                };
            }
            LuceneSearchQueryWithFiltersAndFacets searchDescriptor = _examineQueryTranslatorService.Translate(examineIndex,query) as LuceneSearchQueryWithFiltersAndFacets; 
           var searcher = searchDescriptor.Execute();
           if (_configuration.EnableDebugInformation)
           {
               _logger.LogInformation(searchDescriptor.ToString());
           }
           var resultModel = new SimplQueryResult()
           {
               SearchModels = searcher.Select(x=>TranslateModel(x)).ToList(),
             //  Aggregations = TranslateAggregations(result.Aggregations),
               Pagination = new Pagination()
               {
                   CurrentPage = query.Page,
                   PageSize = query.PageSize,
                   Total = searcher.TotalItemCount,
                   TotalNumberOfPages = (int)Math.Ceiling((searcher.TotalItemCount / (double)query.PageSize))
               },
            //   HighLighter = TranslateHighLighter(result.Hits)
           };
           return resultModel;
            
            //todo: implement when translation done
            return new SimplQueryResult()
            {
                SearchModels = new List<ISearchModel>(),
                Pagination = new Pagination()
                {
                    Total = 0,
                    TotalNumberOfPages = 0
                },
                HighLighter = new HighLighter()
            };
        }

      

        private ISearchModel  TranslateModel(ISearchResult commandModels)
        {
           
                var translatedModel = new BaseSearchModel();
                translatedModel.Id = commandModels.Id;
                translatedModel.ContentType = commandModels.Values["contentType"];
                translatedModel.Culture = new CultureInfo( commandModels.Values["culture"].ToString());
                translatedModel.Facet = commandModels.Values["facet"].ToString();
//todo: figure out mapping back rest of properties       
            return translatedModel;
        }
    }
}