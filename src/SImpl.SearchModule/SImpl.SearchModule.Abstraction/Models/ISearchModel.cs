using System;
using System.Collections.Generic;
using System.Globalization;

namespace SImpl.SearchModule.Abstraction.Models
{
    public interface ISearchModel
    {
        DateTime? IndexedAt { get; set; }
        CultureInfo Culture { get; set; }
        string ContentKey { get; set; }
        string Content { get; set; }
        IList<string> Tags { get; set; }
        string ContentType { get; set; }
        Type ViewModelType { get; set; }
        IDictionary<string, List<object>> CustomProperties { get; set; }

    }
}