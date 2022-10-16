namespace SImpl.SearchModule.Algolia.Configuration
{
    public class AlgoliaSearchConfiguration
    {
        public string IndexPrefixName { get; set; } = "";
        public List<string> IndexName { get; set; } = new List<string>();

        public Type LuceneDirectoryFactory { get; set; } = typeof(SyncedFileSystemDirectoryFactory);
        public FieldDefinitionCollection FieldsDefinition { get; set; }
        public bool EnableDebugInformation { get; set; }
        public Type SearchService { get; set; }
    }
}