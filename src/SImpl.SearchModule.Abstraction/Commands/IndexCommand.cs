
using System.Collections.Generic;
using SImpl.CQRS.Commands;
using SImpl.SearchModule.Abstraction.Models;

namespace SImpl.SearchModule.Abstraction.Commands
{
    public class IndexCommand : ICommand
    {
        public List<ISearchModel> Models { get; set; }      
        public string Index { get; set; }
    }
}