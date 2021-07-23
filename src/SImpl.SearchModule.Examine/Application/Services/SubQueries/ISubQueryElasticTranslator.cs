using System;
using System.Collections.Generic;
using Examine.Search;
using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.Examine.Application.Services.SubQueries
{
    public interface ISubQueryExamineTranslator<T> : ISubQueryExamineTranslator where T : ISearchSubQuery
    {
     
    }

    public interface ISubQueryExamineTranslator
    {
        INestedBooleanOperation Translate<TViewModel>(IEnumerable<ISubQueryExamineTranslator> collection, ISearchSubQuery query, IQuery luceneQuery) where TViewModel : class;
    }
}