namespace SImpl.SearchModule.Abstraction.Queries.subqueries
{
    public class PrefixPhraseSubQuery: ISearchSubQuery
    {
        public Occurance Occurance { get; set; }
        public int BoostValue { get; set; } = 1;
        public string Field { get; set; }
        public object Value { get; set; }
        public int? Limit { get; set; } = 5000;
    }
}