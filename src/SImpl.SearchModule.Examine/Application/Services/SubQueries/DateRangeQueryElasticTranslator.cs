﻿using System;
using System.Collections.Generic;
using Examine;
using Examine.Lucene.Providers;
using Examine.Lucene.Search;
using Examine.Search;
using Lucene.Net.Search;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;
using SImpl.SearchModule.Examine.Application.LuceneEngine;

namespace SImpl.SearchModule.Examine.Application.Services.SubQueries
{
    public class DateRangeQueryElasticTranslator : ISubQueryElasticTranslator<DateRangeQuery>
    {
        public Query Translate<TViewModel>(ISearcher searcher, IEnumerable<ISubQueryElasticTranslator> collection,
            ISearchSubQuery query) where TViewModel : class
        {
            var searcherBase = searcher as BaseLuceneSearcher;
            var nestedQuery = new LuceneSearchQueryWithFiltersAndFacets(searcherBase.GetSearchContext(), "baseSearch",
                searcherBase.LuceneAnalyzer, new LuceneSearchOptions(), MapOccuranceToExamine(query.Occurance));
            var termSubQuery = (DateRangeQuery)query;
            nestedQuery.RangeQuery<DateTime>(
                new[] { termSubQuery.Field },
                termSubQuery.MinValue,
                termSubQuery.MaxValue,
                maxInclusive: termSubQuery.IncludeMaxEdge, minInclusive: termSubQuery.IncludeMinEdge);
            return nestedQuery.Query;
        }

        private BooleanOperation MapOccuranceToExamine(Occurance queryOccurance)
        {
            switch (queryOccurance)
            {
                case Occurance.Should:
                    return BooleanOperation.Or;
                case Occurance.Must:
                    return BooleanOperation.And;
                case Occurance.MustNot:
                    return BooleanOperation.Not;
            }

            return BooleanOperation.Or;
        }
    }
}