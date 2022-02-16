using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.HighlightQueries;
using SImpl.SearchModule.Abstraction.Results;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public class HighLighterConfigurator
    {
        public HighLighterConfigurator(ISearchQuery<IQueryResult> searchSubQueries)
        {
            searchSubQueries  .HighlightQueries.Add(this.Query);
            this.Query = new BaseHighlightQuery();
        }

        public IHighlightQuery Query { get; set; }
    }
}