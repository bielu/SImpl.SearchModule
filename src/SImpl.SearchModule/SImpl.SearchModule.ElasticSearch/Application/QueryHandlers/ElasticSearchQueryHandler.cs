using System.Collections.Generic;
using System.Linq;
using Nest;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.ElasticSearch.Application.QueryHandlers
{
    public class ElasticSearchQueryHandler : IElasticSearchQueryHandler
    {
         private QueryContainer BuildQuery(ISearchQuery query)
                 {
                     var mustQueries = new List<QueryContainer>();
                     var filterQueries = new List<QueryContainer>();
                     var shouldQueries = new List<QueryContainer>();

                     if (!string.IsNullOrEmpty(query.Culture))
                     {
                         mustQueries.Add(new TermQuery
                         {
                             Field = Infer.Field<ISearchModel>(f => f.Culture),
                             Value = query.Culture.ToLower(),
                         });
                     }

                     foreach (var field in query.Where(x=>x.Key == Occurance.Must))
                     {
                         mustQueries.Add(new TermQuery
                         {
                             Field = Infer.Field<ISearchModel>(model => model.GetType().GetProperties().FirstOrDefault(e=>e.Name ==field.Value.Field) ),
                             Value = field.Value.Query,
                         });
                     }

                     var finalQuery = new BoolQuery
                     {
                         Must = mustQueries,
                         Should = shouldQueries,
                         Filter = filterQueries,
                         MinimumShouldMatch = shouldQueries.Any() ? 1 : 0,
                     };
         
                     return finalQuery;
                 }
    }
}