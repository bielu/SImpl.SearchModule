using System;
using Examine;
using SImpl.SearchModule.Examine.Application.Configuration;

namespace SImpl.SearchModule.Examine.Configuration
{
    public class ExamineSearchConfiguration
    {
        public Type LuceneDirectoryFactory { get; set; }
        public FieldDefinitionCollection FieldsDefinition { get; set; }
    }
}