using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Nest;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.ElasticSearch.Application;
using SImpl.SearchModule.ElasticSearch.Application.CommandHandlers;
using SImpl.SearchModule.ElasticSearch.Application.Services;
using SImpl.SearchModule.ElasticSearch.Configuration;
using SImpl.SearchModule.ElasticSearch.Models;

namespace SImpl.SearchModule.Core.Application.Services
{
    public class DirectIndexingService : IIndexingService
    {
        private readonly IElasticClient _client;
        private readonly ElasticSearchConfiguration _elasticSearchConfiguration;
        private readonly IElasticMapper _elasticMapper;
        private readonly ILogger<IndexCommandHandler> _logger;

        public DirectIndexingService(IElasticClient client, ElasticSearchConfiguration elasticSearchConfiguration,
            IElasticMapper elasticMapper, ILogger<IndexCommandHandler> logger)
        {
            _client = client;
            _elasticSearchConfiguration = elasticSearchConfiguration;
            _elasticMapper = elasticMapper;
            _logger = logger;
        }
        public void Index(List<ISearchModel> searchModels, string index)
        {
         var indexAlias = _elasticSearchConfiguration.IndexPrefixName + index.ToLowerInvariant();
   
            var indexName = (!_elasticSearchConfiguration.UseZeroDowntimeIndexing
                ? indexAlias
                : indexAlias + DateTime.Now.ToString("-dd-MMM-HH-mm-ss")).ToLowerInvariant();
            if (_elasticSearchConfiguration.UseZeroDowntimeIndexing)
            {
                var indexDef =  _client.Indices.AliasExists(indexAlias);
                if (!indexDef.Exists)
                {
                    var answer =  _client.Indices.Create(indexName, index =>
                    {
                        index = index.Aliases(x => x.Alias(indexAlias));


                        return index.Map(f => _elasticMapper.Map(f));
                    });
                     _client.Indices.PutAlias(indexName, indexAlias);
                }
                else
                {
                    var alias =  _client.Indices.GetAlias(indexAlias);
                    indexName = alias.Indices.FirstOrDefault().Key.Name;
                }
            }
            else
            {
                var IndexDef =  _client.Indices.Exists(indexAlias);
                if (!IndexDef.Exists)
                {
                    var answer =  _client.Indices.CreateAsync(indexName, index =>
                    {
                        if (_elasticSearchConfiguration.UseZeroDowntimeIndexing)
                        {
                            index = index.Aliases(x => x.Alias(indexAlias));
                        }

                        return index.Map(f => _elasticMapper.Map(f));
                    });
                }
            }

            var answerIndex =  _client.Bulk(x =>
                x.IndexMany<ElasticSearchModel>(searchModels.Select(ElasticSearchModelMapper.Map)
                    .ToList(), (bulkDes, record) => bulkDes
                    .Index(indexName)
                    .Document(record)));
            if (answerIndex.Errors)
            {
                _logger.LogError(answerIndex.DebugInformation);
                throw new Exception(answerIndex.DebugInformation);
            }
        }

        public void Delete(List<Guid> searchModels, string index)
        {
        var indexAlias =_elasticSearchConfiguration.IndexPrefixName + index.ToLowerInvariant();
 

            var indexDef =  _client.Indices.Exists(indexAlias);
            if (!indexDef.Exists)
            {
                var answer = _client.Indices.Create(indexAlias, index =>
                {
                    if (_elasticSearchConfiguration.UseZeroDowntimeIndexing)
                    {
                        index = index.Aliases(x => x.Alias(indexAlias));
                    }

                    return index.Map(f => _elasticMapper.Map(f));
                });
            }

            var answerIndex=  _client.Bulk(x => 
                x.DeleteMany<ElasticSearchModel>(searchModels.Select(x=>new ElasticSearchModel()
                    {
                        Id = x.ToString()
                    })
                    .ToList(), (bulkDes, record) => bulkDes
                    .Index(indexAlias)
                    .Document(record)));
            if (answerIndex.Errors)
            {
                throw new Exception(answerIndex.DebugInformation);
            } 
        }

        public void Delete(List<string> searchModels, string index)
        {
            var indexName =_elasticSearchConfiguration.IndexPrefixName + index.ToLowerInvariant();
        ;

            var indexDef =  _client.Indices.Exists(indexName);
            if (!indexDef.Exists)
            {
                var answerIndex = _client.Bulk(x =>
                    x.DeleteMany<ElasticSearchModel>(searchModels.Select(x => new ElasticSearchModel()
                        {
                            Id = x
                        })
                        .ToList(), (bulkDes, record) => bulkDes
                        .Index(index)
                        .Document(record)));
                if (answerIndex.Errors)
                {
                    throw new Exception(answerIndex.DebugInformation);
                }
            }
        }

        public void Delete(List<ISearchModel> searchModels, string index)
        {
            var indexAlias =_elasticSearchConfiguration.IndexPrefixName + index.ToLowerInvariant();
     
            var answerIndex=  _client.Bulk(x => 
                x.DeleteMany<ElasticSearchModel>(searchModels.Select(ElasticSearchModelMapper.Map)
                    .ToList(), (bulkDes, record) => bulkDes
                    .Index(indexAlias)
                    .Document(record)));
            if (answerIndex.Errors)
            {
                throw new Exception(answerIndex.DebugInformation);
            } 
        }
    }
}