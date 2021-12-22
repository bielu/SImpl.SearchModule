using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Nest;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;

namespace SImpl.SearchModule.ElasticSearch.Application.Services.SubQueries
{
    public class BooleanSubQueryElasticTranslator : ISubQueryElasticTranslator<BoolSearchSubQuery>
    {
      
        public QueryContainerDescriptor<TViewModel> Translate<TViewModel>(IEnumerable<ISubQueryElasticTranslator> collection, ISearchSubQuery query) where TViewModel : class
        {
            var boolQuery = (BoolSearchSubQuery)query;
            
            var queryContainer = new QueryContainerDescriptor<TViewModel>();
            var booleanQueryContainer = new BoolQueryDescriptor<TViewModel>();
            var mustQueries =new List<QueryContainerDescriptor<TViewModel>>();  
            var shouldQueries =new List<QueryContainerDescriptor<TViewModel>>();  
            var filterQueries  =new List<QueryContainerDescriptor<TViewModel>>();  
            var mustNotQueries =new List<QueryContainerDescriptor<TViewModel>>();  

            foreach (var subQuery in boolQuery.NestedQueries)
            {
                var type = subQuery.GetType();
                var handlerType = typeof(ISubQueryElasticTranslator<>).MakeGenericType(type);
                var translator =
                    collection.FirstOrDefault(x => x.GetType().GetInterfaces().Any(x => x == handlerType));
                if (translator == null)
                {
                    continue;
                }
                switch (subQuery.Occurance)
                {
                    //That is overcomplicated, but we are abstracting out all layers of query, so dont think so there exists simpler way, but happy to change
                    case Occurance.Filter:
                        filterQueries.Add( translator.Translate<TViewModel>(collection,subQuery) );
                        break;
                    case Occurance.Must:
                        mustQueries.Add( translator.Translate<TViewModel>(collection,subQuery) );;
                        break;
                    case Occurance.MustNot:
                        mustNotQueries.Add( translator.Translate<TViewModel>(collection,subQuery) );;
                        break;
                    case Occurance.Should:
                        shouldQueries.Add( translator.Translate<TViewModel>(collection,subQuery) );;
                        break;
                }
            }
            booleanQueryContainer.Filter(filterQueries.ToArray());
            booleanQueryContainer.Should(shouldQueries.ToArray());
            booleanQueryContainer.Must(mustQueries.ToArray());
            booleanQueryContainer.MustNot(mustNotQueries.ToArray());
            booleanQueryContainer.Boost(boolQuery.BoostValue);
            queryContainer.Bool(b => booleanQueryContainer);
            return queryContainer;
        }

        
    }
}