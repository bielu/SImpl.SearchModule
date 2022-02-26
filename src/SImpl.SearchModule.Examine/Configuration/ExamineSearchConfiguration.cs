using System;
using Examine;
using J2N.Collections.Generic;

namespace SImpl.SearchModule.Examine.Configuration
{
    public class ExamineSearchConfiguration
    {
        public string IndexPrefixName { get; set; } = "";
        public List<string> IndexName { get; set; } = new List<string>();

        public Type LuceneDirectoryFactory { get; set; }
        public FieldDefinitionCollection FieldsDefinition { get; set; }
        public bool EnableDebugInformation { get; set; }
    }
}