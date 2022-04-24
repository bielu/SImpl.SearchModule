using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public interface IAggregationConfigurator : IQueryConfigurator
    {
        public FilterAggregationQuery FilterAggregationQuery { get; }
    }
    public interface IQueryConfigurator
    {
        public INestableQuery Query { get; set; }
        Occurance Occurance { get; set; }
    }
}