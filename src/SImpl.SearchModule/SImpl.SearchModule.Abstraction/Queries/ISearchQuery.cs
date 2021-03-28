using System;
using System.Collections.Generic;
using SImpl.SearchModule.Abstraction.Fields;

namespace SImpl.SearchModule.Abstraction.Queries
{
    public interface ISearchQuery : IDictionary<Occurance, ISearchSubQuery>, ICreatableSearchQuery<Occurance, ISearchSubQuery>
    {
        public string Culture { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<ISortOrderField> SortOrder { get; set; } 
        public List<IFacetField> FacetFields { get; set; }
        public DateTime? PreviewAt { get; set; }
        public bool Preview { get; set; }
        public string Term { get; set; }
        void Add(Occurance queryOccurance, ISearchSubQuery booleanQueryQuery);
    }

}