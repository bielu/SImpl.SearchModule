using System;

namespace SImpl.SearchModule.Abstraction.Queries
{
    public class FuzzyQuery : ISearchSubQuery
    {
        public Occurance Occurance { get; set; }
        public int BoostValue { get; set; } = 1;
        public string Field { get; set; }
        public object Value { get; set; }
        public bool Transposition { get; set; }
        public int PrefixLenght { get; set; }
        public int MaxExpansions { get; set; }
        public Tuple<int,int> Fuziness { get; set; }
    }
}