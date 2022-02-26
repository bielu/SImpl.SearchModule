namespace SImpl.SearchModule.Examine.Models
{
    public class TextElasticProperty : IElasticProperty
    {
        public string Name { get; set; }
        public double? Boost { get; set; }
        public bool? SplitQueriesOnWhiteSpace { get; set; }
    }
    public class DateElasticProperty : IElasticProperty
    {
        public string Name { get; set; }
        public double? Boost { get; set; }
        public bool? SplitQueriesOnWhiteSpace { get; set; }
        public string Format { get; set; }
    }
    public interface IElasticProperty
    {
        string Name { get; set; }
    }
}