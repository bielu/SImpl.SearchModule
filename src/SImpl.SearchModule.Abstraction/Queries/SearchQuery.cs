using System;
using System.Collections.Generic;
using SImpl.CQRS.Queries;
using SImpl.SearchModule.Abstraction.Fields;
using SImpl.SearchModule.Abstraction.Results;

namespace SImpl.SearchModule.Abstraction.Queries
{
    public class SearchQuery : Dictionary<Occurance, ISearchSubQuery>, ISearchQuery<IQueryResult>, IQuery<IQueryResult>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<ISortOrderField> SortOrder { get; set; } = new List<ISortOrderField>();
        public List<IFacetField> FacetFields { get; set; } = new List<IFacetField>();
        public string Index { get; set; }
        public DateTime? PreviewAt { get; set; }
    }
}