using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMineLib.Interfaces
{
    internal interface IGrokPatternsManager
    {
        void LoadPatterns();
        IList<string> GetAllGrokKeys();
        string GetPattern(string GrokExpression);
    }
}
