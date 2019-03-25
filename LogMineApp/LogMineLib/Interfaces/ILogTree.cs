using LogMineLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMineLib.Interfaces
{
    public interface ILogTree
    {
        IList<LineNode> BuildLogTree(); 
        IList<string> GetAllLogLines();
        IList<string> ExtractPatternOutput();
    }
}
