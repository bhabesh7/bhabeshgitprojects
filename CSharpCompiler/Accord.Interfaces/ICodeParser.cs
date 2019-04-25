using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accord.Interfaces
{
    public interface ICodeParser
    {
        string GetCodeFromFile(string filePath);
    }
}
