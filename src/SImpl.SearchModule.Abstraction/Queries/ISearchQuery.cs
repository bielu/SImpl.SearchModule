using System;
using System.Collections.Generic;
using SImpl.CQRS.Queries;
using SImpl.SearchModule.Abstraction.Fields;
using SImpl.SearchModule.Abstraction.Queries.HighlightQueries;

namespace SImpl.SearchModule.Abstraction.Queries
{
    public interface ISearchQuery<T> : IDictionary<Occurance, ISearchSubQuery>, ICreatableSearchQuery<Occurance, ISearchSubQuery>, IQuery<T>, ISearchQuery
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<ISortOrderField> SortOrder { get; set; } 
        public List<IAggregationQuery> FacetQueries { get; set; }
        public List<IHighlightQuery> HighlightQueries { get; set; }
        public  IDictionary<Occurance, ISearchSubQuery> PostFilterQuery { get; set; }
        public string Index { get; set; }
        public DateTime? PreviewAt { get; set; }
        void Add(Occurance queryOccurance, ISearchSubQuery booleanQueryQuery);
    }

    public interface ISearchQuery
    {
        
    }
}