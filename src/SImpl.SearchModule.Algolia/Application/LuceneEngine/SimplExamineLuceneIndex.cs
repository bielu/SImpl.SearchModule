namespace SImpl.SearchModule.Algolia.Application.LuceneEngine
{
    public class SimplExamineLuceneIndex : LuceneIndex
    {
        private readonly Lazy<LuceneSearcher> _searcher;
        private ControlledRealTimeReopenThread<IndexSearcher> _nrtReopenThread;
        public SimplExamineLuceneIndex(ILoggerFactory loggerFactory, string name, IOptionsMonitor<LuceneDirectoryIndexOptions> indexOptions) : base(loggerFactory, name, indexOptions)
        {
            _searcher = new Lazy<LuceneSearcher>(CreateSearcher);
        }
        public override ISearcher Searcher => _searcher.Value;
        private LuceneSearcher CreateSearcher()
        {
            var possibleSuffixes = new[] { "Index", "Indexer" };
            var name = Name;
            foreach (var suffix in possibleSuffixes)
            {
                //trim the "Indexer" / "Index" suffix if it exists
                if (!name.EndsWith(suffix))
                    continue;
                name = name.Substring(0, name.LastIndexOf(suffix, StringComparison.Ordinal));
            }

            TrackingIndexWriter writer = IndexWriter;
            var searcherManager = new SearcherManager(writer.IndexWriter, true, new SearcherFactory());
            searcherManager.AddListener(this);

            _nrtReopenThread = new ControlledRealTimeReopenThread<IndexSearcher>(writer, searcherManager, 5.0, 1.0)
            {
                Name = $"{Name} NRT Reopen Thread",
                IsBackground = true
            };

            _nrtReopenThread.Start();

            // wait for most recent changes when first creating the searcher
            WaitForChanges();

            return new SimplExamineLuceneSearcher(name + "Searcher", searcherManager, FieldAnalyzer, FieldValueTypeCollection);
        }
    }
}