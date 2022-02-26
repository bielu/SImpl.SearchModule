using System;
using System.Globalization;
using System.Linq;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.Examine.Models;

namespace SImpl.SearchModule.Examine.Application
{
    public class ElasticSearchModelMapper
    {
        public static string MapToSimpleTypeName(Type type)
        {
            return $"{type.FullName}, {type.Assembly.GetName().Name}";
        }

        public static ElasticSearchModel Map(ISearchModel model)
        {
            return new ElasticSearchModel
            {
                Id = model.ContentKey,
                AdditionalKeys = model.AdditionalKeys,
                Culture = model.Culture.IetfLanguageTag.ToLower(),
                Content = model.Content,
                Facet = model.Facet,
                ContentType = model.ContentType,
                Tags = model.Tags,
                IndexedAt = model.IndexedAt,
                ViewModelType = MapToSimpleTypeName(model.ViewModelType),
                CustomProperties = model.CustomProperties,
            };
        }

        public static ISearchModel Map(ElasticSearchModel model)
        {
            return new BaseSearchModel
            {
                ContentKey = model.Id,
                Content = model.Content,
                AdditionalKeys = model.AdditionalKeys,
                ContentType = model.ContentType,
                Facet = model.Facet,
                Culture = new CultureInfo(model.Culture),
                IndexedAt = model.IndexedAt,
                Tags = model.Tags?.ToList(),
                ViewModelType = Type.GetType(model.ViewModelType),
                CustomProperties = model.CustomProperties,
            };
        }
    }
}