using System.Collections.Generic;

namespace Simpl.SearchModule.TestApi.Models
{
    public class BasicFilter
    {
        public string Key { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }

        public long? TotalResults { get; set; }
        public IList<BasicFilterOption> Options { get; set; } = (IList<BasicFilterOption>) new List<BasicFilterOption>();
    }

    public class BasicFilterOption
    {
        public string OptionId { get; set; }

        public string OptionName { get; set; }

        public bool Selected { get; set; }

        public int Count { get; set; }

        public IList<BasicFilterOption> Options { get; set; } = (IList<BasicFilterOption>) new List<BasicFilterOption>();
    }
}