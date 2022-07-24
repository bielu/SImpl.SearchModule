﻿using System.Collections.Generic;

namespace SImpl.SearchModule.Abstraction.Queries.subqueries
{
    public class BoolSearchSubQuery : INestableQuery
    {
        public Occurance Occurance { get; set; } = Occurance.Must;
        public string Field { get; set; }
        public int BoostValue { get; set; } = 1;
        public List<ISearchSubQuery> NestedQueries { get; set; } = new List<ISearchSubQuery>();
        public void Add(Occurance key, ISearchSubQuery value)
        {
            NestedQueries.Add(value);
        }
    }
}