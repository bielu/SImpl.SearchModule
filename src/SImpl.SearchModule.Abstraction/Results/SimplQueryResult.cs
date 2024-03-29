﻿using System.Collections.Generic;
using SImpl.SearchModule.Abstraction.Models;

namespace SImpl.SearchModule.Abstraction.Results
{
    public class SimplQueryResult : IQueryResult
    {
        public List<ISearchModel> SearchModels { get; set; }
        public List<IAggregation> Aggregations { get; set; } = new List<IAggregation>();
        public bool IsSuccess { get; set; } = false;
        public string ErrorMessage { get; set; }
        public string Culture { get; set; }     
        public string Term { get; set; }
        public Pagination Pagination { get; set; }
        public List<Sort> Sorts { get; set; }
        public HighLighter HighLighter { get; set; } = new HighLighter();
    }
}