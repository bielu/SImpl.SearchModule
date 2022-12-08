﻿using System;
using System.Globalization;
using System.Linq;
using SImpl.SearchModule.Abstraction.Models;
using SImpl.SearchModule.ElasticSearch.Models;

namespace SImpl.SearchModule.ElasticSearch.Application
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
                Key = model.Key,
                Url = model.Url,
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
                Key = model.Key,
                Url = model.Url,
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