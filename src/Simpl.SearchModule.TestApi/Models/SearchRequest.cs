using System.Collections;
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


        public IEnumerable<string> ContentTypes { get; set; } = (IEnumerable<string>) new List<string>();

        public int Sort { get; set; } = 0;
        public IEnumerable<BasicFilter> Filters { get; set; } = (IEnumerable<BasicFilter>) new List<BasicFilter>();

        public IEnumerable<BasicFilter> PreFilters { get; set; } = (IEnumerable<BasicFilter>) new List<BasicFilter>();

        public IEnumerable<BasicFacet> Facets { get; set; } = (IEnumerable<BasicFacet>) new List<BasicFacet>();

    }
}