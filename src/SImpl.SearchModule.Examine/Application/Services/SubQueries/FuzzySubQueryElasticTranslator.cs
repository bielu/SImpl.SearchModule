using System.Collections.Generic;
using SImpl.SearchModule.Abstraction.Queries;
using FuzzyQuery = SImpl.SearchModule.Abstraction.Queries.FuzzyQuery;

namespace SImpl.SearchModule.Examine.Application.Services.SubQueries
{
    public class FuzzySubQueryElasticTranslator : ISubQueryElasticTranslator<FuzzyQuery>
    {
        public QueryContainerDescriptor<TViewModel> Translate<TViewModel>(
            IEnumerable<ISubQueryElasticTranslator> collection, ISearchSubQuery query) where TViewModel : class
        {
            var castedQuery = (FuzzyQuery)query;
            var queryResult = new QueryContainerDescriptor<TViewModel>();
            var fuziness = Fuzziness.AutoLength(castedQuery.Fuziness.Item1, castedQuery.Fuziness.Item2);
            queryResult.Fuzzy(x => x.Field(new Field(castedQuery.Field)).Value(castedQuery.Value.ToString())
                .Fuzziness(
                fuziness).Boost(castedQuery.BoostValue));
            return queryResult;
        }
    }
}