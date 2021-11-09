using System;
using System.Collections.Generic;
using SImpl.Common;

namespace SImpl.SearchModule.ElasticSearch.Models
{
    public class ElasticSearchModel : IEntity<string>
    {
        public string Id { get; set; }
        public DateTime? IndexedAt { get; set; }
        public string Culture { get; set; } 
        public string Content { get; set; }
        public IEnumerable<string> Tags { get; set; } = new List<string>();
        public string ContentType { get; set; }
        public string ViewModelType { get; set; }
        public IDictionary<string, List<object>> CustomProperties { get; set; } = new Dictionary<string, List<object>>();
    }
}