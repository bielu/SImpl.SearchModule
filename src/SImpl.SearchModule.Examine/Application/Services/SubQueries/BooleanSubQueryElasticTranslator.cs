using System;
using System.Collections.Generic;
using System.Linq;
using Examine.Lucene.Search;
using Examine.Search;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;

namespace SImpl.SearchModule.Examine.Application.Services.SubQueries
{
    public class BooleanSubQueryElasticTranslator : ISubQueryExamineTranslator<BoolSearchSubQuery>
    {
      
        public INestedBooleanOperation Translate<TViewModel>(IEnumerable<ISubQueryExamineTranslator> collection, ISearchSubQuery query, IQuery luceneQuery) where TViewModel : class
        {
            var boolQuery = (BoolSearchSubQuery)query;
            var luceneSearchQuery = luceneQuery as LuceneSearchQuery;
            INestedBooleanOperation queryContainer = new LuceneBooleanOperation(luceneSearchQuery);

            foreach (var subQuery in boolQuery.NestedQueries)
            {
                var type = subQuery.GetType();
                var handlerType = typeof(ISubQueryExamineTranslator<>).MakeGenericType(typeof(ISubQueryExamineTranslator<>), type);
                var translator =
                    collection.FirstOrDefault(x => x.GetType().GetGenericTypeDefinition() == handlerType);
                if (translator == null)
                {
                    continue;
                }

                switch (subQuery.Occurance)
                {
                    //That is overcomplicated, but we are abstracting out all layers of query, so dont think so there exists simpler way, but happy to change
                    case Occurance.Filter:
                        queryContainer=   queryContainer.And(e=>translator.Translate<TViewModel>(collection, subQuery,  (IQuery)e));
                        break;
                    case Occurance.Must:
                        queryContainer.And(e=>translator.Translate<TViewModel>(collection,subQuery, (IQuery)e) );
                        break;
                    case Occurance.MustNot:
                        queryContainer.AndNot(e=>translator.Translate<TViewModel>(collection,subQuery,  (IQuery)e) );;
                        break;
                    case Occurance.Should:
                        queryContainer.And(e =>translator.Translate<TViewModel>(collection,subQuery,  (IQuery)e), BooleanOperation.Or );;
                        break;
                }
            }
            return queryContainer;
        }

        
    }
}