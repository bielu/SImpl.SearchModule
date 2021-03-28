using System;
using System.Collections.Generic;
using SImpl.SearchModule.Abstraction.Fields;

namespace SImpl.SearchModule.Abstraction.Queries
{
    public class SearchQuery : Dictionary<Occurance, ISearchSubQuery>, ISearchQuery
    {
        public string Culture { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<ISortOrderField> SortOrder { get; set; }
        public List<IFacetField> FacetFields { get; set; }
        public DateTime? PreviewAt { get; set; }
        public bool Preview { get; set; }
        public string Term { get; set; }
    }
}