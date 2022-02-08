using System.Collections.Generic;

namespace SImpl.SearchModule.Abstraction.Queries.subqueries
{
    public class TermsSubQuery : ISearchSubQuery
    {
        public Occurance Occurance { get; set; }
        public int BoostValue { get; set; }
        public string Field { get; set; }
        public List<object> Value { get; set; }
    }
}