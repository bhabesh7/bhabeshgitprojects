using ImageFastLoader.DataModel;
using ImageFastLoader.LuceneServices.Inte;
using Lucene.Net.Store;


using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageFastLoader.LuceneServices.Impl
{
    public class FileScanner : IFilesScanner
    {
        public IList<DocumentData> GetFileListWithFullPath(string location)
        {
            IList<DocumentData> documentList = new List<DocumentData>();
            if(string.IsNullOrEmpty(location))
            {
                return null;
            }
            var filePathList =  System.IO.Directory.EnumerateFiles(location).ToList();
            foreach (var item in filePathList)
            {
                documentList.Add(new DocumentData { FileName = Path.GetFileName(item), FilePath = item });
            }

            return documentList;

        }
    }
}
