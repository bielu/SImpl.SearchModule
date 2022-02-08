using System.Collections.Generic;

namespace SImpl.SearchModule.Abstraction.Models
{
    public class MultiTermBucket
    {
        public List<string> Keys { get; set; }
        public string Key { get; set; }
        public long? TotalOfDocuments { get; set; }
    }
}