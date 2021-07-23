namespace SImpl.SearchModule.Abstraction.Queries
{
    public interface IFieldQuery : ISearchSubQuery
    { 
        string Field { get; set; }
        object Query { get; set; }
    }
}