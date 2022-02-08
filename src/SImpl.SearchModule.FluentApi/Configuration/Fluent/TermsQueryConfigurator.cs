using System.Linq;
using SImpl.SearchModule.Abstraction.Queries;
using SImpl.SearchModule.Abstraction.Queries.subqueries;

namespace SImpl.SearchModule.FluentApi.Configuration.Fluent
{
    public class TermsQueryConfigurator
    {
        
        public TermsQueryConfigurator()
        {
            _query = new TermsSubQuery();
        }

        public TermsQueryConfigurator Must()
        {
            Occurance = Occurance.Must;
            return this;
        }
        public TermsQueryConfigurator MustNot()
        {
            Occurance = Occurance.MustNot;
            return this;
        }
        public TermsQueryConfigurator Should()
        {
            Occurance = Occurance.Should;
            return this;
        }

        public TermsQueryConfigurator Filter()
        {
            Occurance = Occurance.Filter;
            return this;
        }

        public TermsQueryConfigurator WithField(string fieldName)
        {
            _query.Field = fieldName;
            return this;  
        }
        public TermsQueryConfigurator WithValue(params object[] value)
        {
            _query.Value = value.ToList();
            return this;
        }

        private TermsSubQuery _query;
        public ISearchSubQuery Query => _query;
        public Occurance Occurance { get; set; }
    }
}