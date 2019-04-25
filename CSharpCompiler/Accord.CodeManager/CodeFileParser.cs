using Accord.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accord.CodeManager
{
    [Export(typeof(ICodeParser))]
    public class CodeFileParser : ICodeParser
    {
        public string GetCodeFromFile(string filePath)
        {
            try
            {
                using (StreamReader sReader = new StreamReader(filePath, Encoding.Default))
                {
                    return sReader.ReadToEnd();
                }
            }
            catch(FileNotFoundException fex)
            { }
            catch (Exception)
            {
                
            }
            return string.Empty;
        }
    }
}
