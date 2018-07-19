using Lucene.Net.Store;
using LuceneSearch.DataModel;
using LuceneSearch.Services.Inte;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneSearch.Services.Impl
{
    public class FileScanner : IFilesScanner
    {
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
                yield return new DocumentData
                {
                    FileName = Path.GetFileNameWithoutExtension(item),
                    Extention = Path.GetExtension(item),
                    FilePath = item
                };
            }

            yield return null;
        }
    }
}
