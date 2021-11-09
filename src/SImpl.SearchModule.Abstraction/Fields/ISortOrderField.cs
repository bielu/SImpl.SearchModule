namespace SImpl.SearchModule.Abstraction.Fields
{
    public interface ISortOrderField
    {
         string FieldName { get; set; }
         bool Desc { get; set; }
        
    }
}