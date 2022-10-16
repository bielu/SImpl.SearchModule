namespace SImpl.SearchModule.Algolia.Application.LuceneEngine
{
    public class SimplExamineLuceneSearcher : LuceneSearcher
    {
        public SimplExamineLuceneSearcher(string name, SearcherManager searcherManager, Analyzer analyzer, FieldValueTypeCollection fieldValueTypeCollection) : base(name, searcherManager, analyzer, fieldValueTypeCollection)
        {
        }
        public override IQuery CreateQuery(string category = null, BooleanOperation defaultOperation = BooleanOperation.And)
            => CreateQuery(category, defaultOperation, LuceneAnalyzer, new LuceneSearchOptions());
        public IQuery CreateQuery(string category, BooleanOperation defaultOperation, Analyzer luceneAnalyzer, LuceneSearchOptions searchOptions)
        {
            if (luceneAnalyzer == null)
                throw new ArgumentNullException(nameof(luceneAnalyzer));

            return new LuceneSearchQueryWithFiltersAndFacets(GetSearchContext(), category, luceneAnalyzer, searchOptions, defaultOperation);
        }
    }
}