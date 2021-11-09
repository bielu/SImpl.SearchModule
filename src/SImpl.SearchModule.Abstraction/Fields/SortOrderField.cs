namespace SImpl.SearchModule.Abstraction.Fields
{
    public class SortOrderField : ISortOrderField
    {
        public string FieldName { get; set; }
        public bool Desc { get; set; }
    }
}