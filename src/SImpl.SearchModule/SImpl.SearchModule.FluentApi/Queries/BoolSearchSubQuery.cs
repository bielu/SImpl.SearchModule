using System.Collections.Generic;
using SImpl.SearchModule.Abstraction.Queries;

namespace SImpl.SearchModule.FluentApi.Queries
{
    public class BoolSearchSubQuery : INestableQuery
    {
        public Occurance Occurance { get; set; } = Occurance.Must;
        public List<ISearchSubQuery> NestedQueries { get; set; }
        public void Add(Occurance key, ISearchSubQuery value)
        {
            NestedQueries.Add(value);
        }
    }
}