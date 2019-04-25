using Accord.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCompiler
{
    public class MEFLoader
    {
        [Import(typeof(IFolderScanner))]
        private IFolderScanner folderScanner;

        [Import(typeof(ICodeParser))]
        private ICodeParser codeParser;

        [Import(typeof(IAnalysisManager))]
        private IAnalysisManager analysisManager;

        const string CSHARP = "C#";
        const string JAVA = "JAVA";
        const string PARTS = "PARTS";
        const string COMPILERLANGUAGE = "CompilerLanguage";

        public void Load()
        {
            var compilerLanguage = ConfigurationManager.AppSettings[COMPILERLANGUAGE].ToString();
            DirectoryCatalog catalog = null;

            switch (compilerLanguage.ToUpper())
            {
                case CSHARP:
                    string csharpPath = Path.Combine(PARTS, CSHARP);
                    catalog = new DirectoryCatalog(csharpPath);
                    break;
                case JAVA:
                    string javaPath = Path.Combine(PARTS, JAVA);
                    catalog = new DirectoryCatalog(javaPath);
                    break;
                default:
                    break;
            }
            CompositionContainer container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        public IAnalysisManager GetAnalysisManager()
        {
            return analysisManager;
        }

        public IFolderScanner GetFolderScanner()
        {
            return folderScanner;
        }

        public ICodeParser GetCodeParser()
        {
            return codeParser;
        }


    }
}
