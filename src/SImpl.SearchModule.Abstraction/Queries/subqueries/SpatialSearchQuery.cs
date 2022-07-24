namespace SImpl.SearchModule.Abstraction.Queries.subqueries
{
    public class SpatialSearchQuery : ISearchSubQuery
    {
        public Occurance Occurance { get; set; }
        public string Field { get; set; }
        public int BoostValue { get; set; }
        public string UnitOfDistance { get; set; }
        public double Distance { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}