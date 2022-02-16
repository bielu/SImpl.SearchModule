
using System.Collections.Generic;
using SImpl.CQRS.Commands;
using SImpl.SearchModule.Abstraction.Models;

namespace SImpl.SearchModule.Abstraction.Commands
{
    public class ReIndexCommand : IndexCommand, ICommand
    {
        
    }
    public class IndexCommand : ICommand
    {
        public List<ISearchModel> Models { get; set; }      
        public string Index { get; set; }
    }
}