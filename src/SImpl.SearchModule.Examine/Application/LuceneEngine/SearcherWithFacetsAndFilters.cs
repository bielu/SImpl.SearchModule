using System.Collections.Generic;
using Examine;
using Examine.Lucene.Search;

namespace SImpl.SearchModule.Examine.Application.LuceneEngine
{
    public class LuceneSearchResultsWithFacetsAndFilters : LuceneSearchResults
    {
        public LuceneSearchResultsWithFacetsAndFilters(IReadOnlyCollection<ISearchResult> results, int totalItemCount) : base(results, totalItemCount)
        {
        }
    }
}