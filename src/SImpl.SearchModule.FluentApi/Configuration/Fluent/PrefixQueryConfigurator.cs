using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;


namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public class PrefixQueryConfigurator
    {
        
            public PrefixQueryConfigurator()
            {
                _query = new TermSubQuery();
            }

            public PrefixQueryConfigurator Must()
            {
                Query.Occurance = Occurance.Must;
                return this;
            }
            public PrefixQueryConfigurator MustNot()
            {
                Query.Occurance = Occurance.MustNot;
                return this;
            }
            public PrefixQueryConfigurator Should()
            {
                Query.Occurance = Occurance.Should;
                return this;
            }

            public PrefixQueryConfigurator Filter()
            {
                Query.Occurance = Occurance.Filter;
                return this;
            }

            public PrefixQueryConfigurator WithField(string fieldName)
            {
                _query.Field = fieldName;
                return this;  
            }
            public PrefixQueryConfigurator WithValue(object value)
            {
                _query.Value = value;
                return this;
            }

            private TermSubQuery _query;
            public ISearchSubQuery Query => _query;
    }
}