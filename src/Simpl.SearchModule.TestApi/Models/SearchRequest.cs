using System.Collections.Generic;

namespace Simpl.SearchModule.TestApi.Models
{
    public class SearchRequest
    {
        public string Term { get; set; } = string.Empty;

        public string Culture { get; set; } = "en-GB".ToLower();

        public int SiteId { get; set; } = 0;

        public int ListingId { get; set; } = 0;

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public IEnumerable<DefaultFilter> Filters { get; set; } = (IEnumerable<DefaultFilter>) new List<DefaultFilter>();

        public IEnumerable<DefaultFilter> PreFilters { get; set; } = (IEnumerable<DefaultFilter>) new List<DefaultFilter>();

        public IEnumerable<DefaultFilter> Facets { get; set; } = (IEnumerable<DefaultFilter>) new List<DefaultFilter>();

        public IEnumerable<string> ContentTypes { get; set; } = (IEnumerable<string>) new List<string>();

        public int Sort { get; set; } = 0;
    }
}