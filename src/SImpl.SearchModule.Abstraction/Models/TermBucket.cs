using System.Collections.Generic;

namespace SImpl.SearchModule.Abstraction.Models
{
    public interface IBucket
    {
        public long? TotalOfDocuments { get; set; }
        public IEnumerable<string> Keys { get; set; }
        public List<IBucket> SubBuckets { get; set; } 
    }
    public class KeyBucket : IBucket
    {
        public string Key { get; set; }
        public long? TotalOfDocuments { get; set; }
        public IEnumerable<string> Keys { get; set; }
        public List<IBucket> SubBuckets { get; set; }
        public bool IsComplex { get; set; }
    }
    public class SingleBucket : IBucket
    {
        public long? TotalOfDocuments { get; set; }
        public IEnumerable<string> Keys { get; set; }
        public List<IBucket> SubBuckets { get; set; }
        public bool IsComplex { get; set; }
    }
}