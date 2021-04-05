using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.ElasticSearch.Application.ElasticQueries;




namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public class TermQueryConfigurator
    {
        
            public TermQueryConfigurator()
            {
                _query = new TermSubQuery();
            }

            public TermQueryConfigurator Must()
            {
                Query.Occurance = Occurance.Must;
                return this;
            }
            public TermQueryConfigurator MustNot()
            {
                Query.Occurance = Occurance.MustNot;
                return this;
            }
            public TermQueryConfigurator Should()
            {
                Query.Occurance = Occurance.Should;
                return this;
            }

            public TermQueryConfigurator Filter()
            {
                Query.Occurance = Occurance.Filter;
                return this;
            }

            public TermQueryConfigurator WithField(string fieldName)
            {
                _query.Field = fieldName;
                return this;  
            }
            public TermQueryConfigurator WithValue(object value)
            {
                _query.Value = value;
                return this;
            }

            private TermSubQuery _query;
            public ISearchSubQuery Query => _query;
    }
}