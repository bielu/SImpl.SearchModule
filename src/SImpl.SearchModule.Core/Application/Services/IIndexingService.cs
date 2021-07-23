using System;
using System.Collections.Generic;
using SImpl.SearchModule.Abstraction.Models;

namespace SImpl.SearchModule.Core.Application.Services
{
    public interface IIndexingService
    {
        void Index(List<ISearchModel> searchModels, string index);
        void Delete(List<Guid> searchModels, string index);
        void Delete(List<string> searchModels, string index);
        void Delete(List<ISearchModel> searchModels, string index);
    }
}