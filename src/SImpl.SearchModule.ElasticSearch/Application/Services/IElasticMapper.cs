using Nest;
using SImpl.SearchModule.ElasticSearch.Configuration;
using SImpl.SearchModule.ElasticSearch.Models;

namespace SImpl.SearchModule.ElasticSearch.Application.Services
{
    public class DefaultElasticMapper : IElasticMapper
    {
        private readonly ElasticSearchConfiguration _configuration;

        public DefaultElasticMapper(ElasticSearchConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ITypeMapping Map(TypeMappingDescriptor<object> typeMappingDescriptor)
        {
            return typeMappingDescriptor.Properties<ElasticSearchModel>(ps =>
            {
                foreach (var propertyGroup in _configuration.ElasticPropertiesFields)
                {
                    foreach (var property in propertyGroup.Value)
                    {
                        switch (propertyGroup.Key)
                        {
                            case AnalyzerType.Keyword:
                                var textProperty = property as TextElasticProperty;
                                ps = ps
                                    .Keyword(s => s
                                        .Name(textProperty.Name)
                                        .Boost(textProperty.Boost)
                                        .SplitQueriesOnWhitespace(textProperty.SplitQueriesOnWhiteSpace)
                                    );
                                break;
                            case AnalyzerType.Binary:
                                ps = ps.Binary(s => s
                                    .Name(property.Name));
                                break;
                            case AnalyzerType.Boolean:
                                ps = ps.Binary(s => s
                                    .Name(property.Name));
                                break;
                            case AnalyzerType.Text:
                                textProperty = property as TextElasticProperty;
                                ps = ps.Text(s => s
                                    .Name(property.Name).Boost(textProperty.Boost));
                                break;
                            case AnalyzerType.Date:
                                var dateProperty = property as DateElasticProperty;
                                ps = ps.Date(s => s
                                    .Name(property.Name).Boost(dateProperty.Boost).Format(dateProperty.Format));
                                break;
                        }
                    }
                }

                return ps;
            }).AutoMap<ElasticSearchModel>();
        }
    }

    public interface IElasticMapper
    {
        public ITypeMapping Map(TypeMappingDescriptor<object> typeMappingDescriptor);
    }
}