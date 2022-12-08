using System;
using System.Collections.Generic;
using System.Globalization;

namespace SImpl.SearchModule.Abstraction.Models
{
    public class BaseSearchModel: ISearchModel
    {
        public string Key { get; set; }
        public string Url { get; set; }
        public DateTime? IndexedAt { get; set; }
        public CultureInfo Culture { get; set; }

        public string SearchCulture
        {
            get
            {
              return  Culture.IetfLanguageTag.ToLower().Replace("-","");
            }
            set { }
        }

        public string ContentKey { get; set; }
        public string Content { get; set; }
        public IList<string> Tags { get; set; } = new List<string>();
        public string ContentType { get; set; }
        public Type ViewModelType { get; set; }

        public IDictionary<string, List<object>> CustomProperties { get; set; } =
            new Dictionary<string, List<object>>();

        public string Id
        {
            get => ContentKey.ToLowerInvariant();
            set => ContentKey = value;
        }

        public string Facet { get; set; }
        public List<string> AdditionalKeys { get; set; } = new List<string>();
    }
}