﻿using System.Collections.Generic;

namespace SImpl.SearchModule.Abstraction.Queries
{
    public class FilterAggregationQuery : IAggregationQuery
    {
        public string AggregationName { get; set; }
        public List<IAggregationQuery> NestedAggregations { get; set; } = new List<IAggregationQuery>();
        public int Size { get; set; } = 1000;

        public Dictionary<Occurance, ISearchSubQuery> Queries { get; set; } =
            new Dictionary<Occurance, ISearchSubQuery>();
    }
}