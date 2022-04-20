using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public class PrefixPhraseQueryConfigurator
    {
        
        public PrefixPhraseQueryConfigurator()
        {
            _query = new PrefixPhraseSubQuery();
        }

        public PrefixPhraseQueryConfigurator Must()
        {
            Query.Occurance = Occurance.Must;
            return this;
        }
        public PrefixPhraseQueryConfigurator MustNot()
        {
            Query.Occurance = Occurance.MustNot;
            return this;
        }
        public PrefixPhraseQueryConfigurator Should()
        {
            Query.Occurance = Occurance.Should;
            return this;
        }

        public PrefixPhraseQueryConfigurator Filter()
        {
            Query.Occurance = Occurance.Filter;
            return this;
        }

        public PrefixPhraseQueryConfigurator WithField(string fieldName)
        {
            _query.Field = fieldName;
            return this;  
        }
        public PrefixPhraseQueryConfigurator WithValue(object value)
        {
            _query.Value = value;
            return this;
        }

        private PrefixPhraseSubQuery _query;
        public ISearchSubQuery Query => _query;
    }
}