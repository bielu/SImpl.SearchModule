using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public interface IQueryConfigurator
    {
        public INestableQuery Query { get; set; }
    }
}