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

namespace Accord.MainApp
{
    public class MEFLoader
    {
        [Import(typeof(IFolderScanner))]
        IFolderScanner folderScanner;

        [Import(typeof(ICodeParser))]
        ICodeParser codeParser;

        [Import(typeof(IAnalysisManager))]
        IAnalysisManager analysisManager;

        const string CSHARP = "C#";
        const string JAVA = "JAVA";
        const string PARTS = "PARTS";
        const string COMPILERLANGUAGE = "CompilerLanguage";

        public void Load()
        {
            try
            {
                var compilerLanguage = ConfigurationManager.AppSettings[COMPILERLANGUAGE].ToString();
                DirectoryCatalog catalog = null;

                switch (compilerLanguage.ToUpper())
                {
                    case CSHARP:
                        string csharpPath = Path.Combine(Environment.CurrentDirectory, PARTS, CSHARP);
                        catalog = new DirectoryCatalog(csharpPath);
                        break;
                    case JAVA:
                        string javaPath = Path.Combine(Environment.CurrentDirectory, PARTS, JAVA);
                        catalog = new DirectoryCatalog(javaPath);
                        break;
                    default:
                        break;
                }
                CompositionContainer container = new CompositionContainer(catalog);
                container.ComposeParts(this);
            }
            catch (Exception ex)
            {                
            }
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
