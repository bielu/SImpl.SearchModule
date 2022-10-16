using Algolia.Search.Models.Search;
using SImpl.CQRS.Queries;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Algolia.Application.LuceneEngine;
using SImpl.SearchModule.Algolia.Application.Services.SubQueries;

namespace SImpl.SearchModule.Algolia.Application.Services
{
    public class BaseAlgoliaQueryTranslatorService : IAlgoliaQueryTranslatorService
    {
        private IEnumerable<ISubQueryElasticTranslator> _collection;
        private readonly IExamineManager _examineManager;

        public BaseAlgoliaQueryTranslatorService(IEnumerable<ISubQueryElasticTranslator> collection, IExamineManager examineManager)
        {
            _collection = collection;
            _examineManager = examineManager;
        }

        public Query Translate<T>(ISearchQuery<T> query)
            where T : class
        {
    
            Query translatedQuery = new Query();

            foreach (var booleanQuery in query)
            {
                var type = booleanQuery.Value.GetType();
                var handlerType =
                    typeof(ISubQueryExamineTranslator<>).MakeGenericType(type);

                var translator =
                    _collection.FirstOrDefault(x => x.GetType().GetInterfaces().Any(x => x == handlerType));
                if (translator == null)
                {
                    continue;
                }
                switch (booleanQuery.Key)
                {
                    case Occurance.MustNot:
                        translatedQuery.Filters+= $"NOT ({ translator.Translate<T>(_collection, booleanQuery.Value)})";
                       
                        break;
                    case Occurance.Must:
                        translatedQuery.Filters+= $"And ({ translator.Translate<T>(_collection, booleanQuery.Value)})";

                        break;
                    case Occurance.Should:
                        translatedQuery.Filters+= $"OR ({ translator.Translate<T>(_collection, booleanQuery.Value)})";
                        break;
                }
            }

            return translatedQuery;

        }

       
    }
}