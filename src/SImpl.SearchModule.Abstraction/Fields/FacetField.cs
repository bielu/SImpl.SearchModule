namespace SImpl.SearchModule.Abstraction.Fields
{
    public class FacetField : IFacetField
    {
        public string FieldName { get; set; }
        public string FacetGroupName { get; set; }
        public int MaxSize { get; set; } = 65536;
    }
}