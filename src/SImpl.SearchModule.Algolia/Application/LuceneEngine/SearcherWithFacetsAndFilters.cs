namespace SImpl.SearchModule.Algolia.Application.LuceneEngine
{
    public class LuceneSearchResultsWithFacetsAndFilters : LuceneSearchResults
    {
        public LuceneSearchResultsWithFacetsAndFilters(IReadOnlyCollection<ISearchResult> results, int totalItemCount) : base(results, totalItemCount)
        {
        }
    }
}