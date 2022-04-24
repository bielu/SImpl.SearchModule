using System.Linq;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public class TermAggregationQueryConfigurator
    {
        public TermAggregation Query;
        public TermAggregationQueryConfigurator()
        {
            Query = new TermAggregation();
        }



        public TermAggregationQueryConfigurator WithName(string name)
        {
            Query.AggregationName = name;
            return this;
        }
        public TermAggregationQueryConfigurator WithField(string name)
        {
            Query.TermFieldName = name;
            return this;
        }
    }
    public class MultiTermAggregationQueryConfigurator
    {
        public MultiTermAggreation Query;
        public MultiTermAggregationQueryConfigurator()
        {
            Query = new MultiTermAggreation();
        }



        public MultiTermAggregationQueryConfigurator WithName(string name)
        {
            Query.AggregationName = name;
            return this;
        }
        public MultiTermAggregationQueryConfigurator WithTerms(params SimpleTerm[] terms)
        {
            Query.Terms = terms.ToList();
            return this;
        }
        public MultiTermAggregationQueryConfigurator WithTerms(params string[] fieldsNames)
        {
            Query.Terms = fieldsNames.Select(x=>new SimpleTerm(){TermFieldName = x}).ToList();
            return this;
        }
    }
}