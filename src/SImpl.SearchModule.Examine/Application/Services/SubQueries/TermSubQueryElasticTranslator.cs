using System.Collections.Generic;
using Examine.Lucene.Search;
using Examine.Search;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;

namespace SImpl.SearchModule.Examine.Application.Services.SubQueries
{
    public class TermSubQueryElasticTranslator : ISubQueryExamineTranslator<TermSubQuery>
    {
        public INestedBooleanOperation Translate<TViewModel>(IEnumerable<ISubQueryExamineTranslator> collection,
            ISearchSubQuery query, IQuery luceneQuery) where TViewModel : class
        {
            var boolQuery = (BoolSearchSubQuery) query;
            var termQuery = query as TermSubQuery;
            var luceneSearchQuery = luceneQuery as LuceneSearchQuery;
            INestedBooleanOperation queryContainer = new LuceneBooleanOperation(luceneSearchQuery);
            //todo: prepare translation of object to examine types if needed
            switch (query.Occurance)
            {
                case Occurance.Filter:
                case Occurance.Must:
                    queryContainer=      queryContainer.And(x => x.Field(termQuery.Field, termQuery.Value.ToString()));
                    break;
                case Occurance.Should:
                    queryContainer=   queryContainer.Or(x => x.Field(termQuery.Field, termQuery.Value.ToString()));
                    break;
                case Occurance.MustNot:
                    queryContainer=  queryContainer.AndNot(x => x.Field(termQuery.Field, termQuery.Value.ToString()));

                    break;
            }

            return queryContainer;

        }
    }
}