namespace SImpl.SearchModule.Abstraction.Queries.subqueries
{
    public class PrefixSubQuery: ISearchSubQuery
    {
        public Occurance Occurance { get; set; }
        public int BoostValue { get; set; } = 1;
        public string Field { get; set; }
        public object Value { get; set; }
    
    }
}