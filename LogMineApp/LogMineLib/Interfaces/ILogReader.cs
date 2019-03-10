using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMineLib.Interfaces
{
    internal interface ILogReader
    {
        IEnumerable<string> ReadLines(string path);
    }
}
