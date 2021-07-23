using System.Collections.Generic;

namespace SImpl.SearchModule.Abstraction.Queries.subqueries
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