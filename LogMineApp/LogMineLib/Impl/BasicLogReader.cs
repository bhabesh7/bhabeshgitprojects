using LogMineLib.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMineLib.Impl
{
    internal class BasicLogReader : ILogReader
    {
        public IEnumerable<string> ReadLines(string path)
        {
            foreach (var line in File.ReadLines(path))
            {
                yield return line;
            }
        }
    }
}
