using System;
using Examine.Lucene;
using Examine.Lucene.Providers;
using Examine.Lucene.Search;
using Examine.Search;
using Lucene.Net.Analysis;
using Lucene.Net.Search;

namespace SImpl.SearchModule.Examine.Application.LuceneEngine
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