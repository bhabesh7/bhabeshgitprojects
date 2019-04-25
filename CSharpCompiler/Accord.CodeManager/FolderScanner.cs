using Accord.DataModel;
using Accord.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accord.CodeManager
{
    [Export(typeof(IFolderScanner))]
    public class FolderScanner : IFolderScanner
    {
        private string _extFilter = ".cs";//default
        const string CSHARP = "C#";
        const string JAVA = "JAVA";
        const string PARTS = "PARTS";
        const string COMPILERLANGUAGE = "CompilerLanguage";

        public FolderScanner()
        {
            var compilerLanguage = ConfigurationManager.AppSettings[COMPILERLANGUAGE].ToString();
            switch (compilerLanguage.ToUpper())
            {
                case CSHARP:
                    _extFilter = ".cs";
                    break;
                case JAVA:
                    _extFilter = ".java";
                    break;
                default:
                    break;
            }
        }
        public IEnumerable<DocumentData> GetFileListWithFullPath(string location)
        {
            //IList<DocumentData> documentList = new List<DocumentData>();
            if (string.IsNullOrEmpty(location))
            {
                yield return null;
            }

            //subdirectories
            var dirs = System.IO.Directory.EnumerateDirectories(location).ToList();

            foreach (var dir in dirs)
            {
                foreach (var item in GetFileListWithFullPath(dir))
                {
                    yield return item;
                }
            }

            var filePathList = System.IO.Directory.EnumerateFiles(location).ToList();
            foreach (var item in filePathList)
            {
                //documentList.Add(new DocumentData { FileName = Path.GetFileName(item), FilePath = item });
                var ext = Path.GetExtension(item);
                if (ext ==_extFilter)
                {
                    yield return new DocumentData
                    {
                        FileName = Path.GetFileNameWithoutExtension(item),
                        Extention = ext,
                        FilePath = item
                    };
                }
            }

            yield return null;
        }

    }
}
