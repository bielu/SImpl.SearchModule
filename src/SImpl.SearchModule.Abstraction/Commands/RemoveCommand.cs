using System;
using System.Collections.Generic;
using SImpl.CQRS.Commands;
using SImpl.SearchModule.Abstraction.Models;

namespace SImpl.SearchModule.Abstraction.Commands
{
    public class RemoveCommand : ICommand
    {
        public string Index { get; set; }
        public List<Guid> ModelsIds { get; set; }
        public List<string> ModelsKeys { get; set; }
        public List<ISearchModel> Models { get; set; }
    }
}