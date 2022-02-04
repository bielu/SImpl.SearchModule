using System;
using System.Collections.Generic;
using System.Globalization;
using SImpl.Common;

namespace SImpl.SearchModule.Abstraction.Models
{
    public interface ISearchModel : IEntity<string>
    {
        DateTime? IndexedAt { get; set; }
        CultureInfo Culture { get; set; }
        string SearchCulture { get; set; }
        string ContentKey { get; set; }
        string Content { get; set; }
        IList<string> Tags { get; set; }
        string ContentType { get; set; }
        Type ViewModelType { get; set; }
        IDictionary<string, List<object>> CustomProperties { get; set; } 
        public string Facet { get; set; }
        List<string> AdditionalKeys { get; set; }
    }
}