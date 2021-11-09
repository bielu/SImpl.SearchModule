namespace SImpl.SearchModule.Abstraction.Models
{
    public class Sort
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool Desc { get; set; }
        public string FieldName { get; set; }
    }

   
}