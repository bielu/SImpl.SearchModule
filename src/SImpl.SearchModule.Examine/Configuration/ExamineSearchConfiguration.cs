using System;
using Examine;

namespace SImpl.SearchModule.Examine.Configuration
{
    public class ExamineSearchConfiguration
    {
        public string IndexPrefixName { get; set; } = "";
        public string IndexName { get; set; } = "SearchIndex";

        public Type LuceneDirectoryFactory { get; set; }
        public FieldDefinitionCollection FieldsDefinition { get; set; }
    }
}