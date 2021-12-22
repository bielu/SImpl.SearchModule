using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public class FuzzyQueryConfigurator
    {
        public FuzzyQueryConfigurator()
        {
            _query = new FuzzyQuery();
        }

        public FuzzyQueryConfigurator Must()
        {
            Query.Occurance = Occurance.Must;
            return this;
        }
        public FuzzyQueryConfigurator MustNot()
        {
            Query.Occurance = Occurance.MustNot;
            return this;
        }
        public FuzzyQueryConfigurator Should()
        {
            Query.Occurance = Occurance.Should;
            return this;
        }

        public FuzzyQueryConfigurator Filter()
        {
            Query.Occurance = Occurance.Filter;
            return this;
        }

        public FuzzyQueryConfigurator WithField(string fieldName)
        {
            _query.Field = fieldName;
            return this;  
        }
        public FuzzyQueryConfigurator WithValue(object value)
        {
            _query.Value = value;
            return this;
        }

        private FuzzyQuery _query;
        public ISearchSubQuery Query => _query;
    }
}