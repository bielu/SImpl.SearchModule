using System.Collections.Generic;

namespace SImpl.SearchModule.Abstraction.Commands
{
    public class IndexMultipleCommand
    {
        public List<IndexCommand> Commands { get; set; }
    }
}