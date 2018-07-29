using Lucene.Net.Store;
using LuceneSearch.DataModel;
using LuceneSearch.Services.Inte;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneSearch.Services.Impl
{
    public class FileScanner : IFilesScanner
    {
        public event EventHandler<FileSystemEventArgs> FileCreatedDeletedEventHandler;
        public event EventHandler<RenamedEventArgs> FileRenamedEventHandler;


        /// <summary>
        /// GetFileListWithFullPath
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
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

        private FileSystemWatcher _fWatcher;

        /// <summary>
        /// SetupFileWatcher
        /// </summary>
        public void SetupFileWatcher()
        {
            var dataLocation = ConfigurationManager.AppSettings.Get("DataLocation");
            _fWatcher = new FileSystemWatcher(dataLocation);
            _fWatcher.EnableRaisingEvents = true;
            _fWatcher.IncludeSubdirectories = true;            
            _fWatcher.Created += FWatcher_CreatedDeleted;
            _fWatcher.Deleted += FWatcher_CreatedDeleted;
            _fWatcher.Renamed += _fWatcher_Renamed;
        }
               
        private void _fWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            if (FileRenamedEventHandler != null)
            {
                FileRenamedEventHandler(this, e);
            }
        }

        private void FWatcher_CreatedDeleted(object sender, FileSystemEventArgs e)
        {
            switch (e.ChangeType)
            {                
                case WatcherChangeTypes.Created:
                case WatcherChangeTypes.Deleted:
                    if (FileCreatedDeletedEventHandler !=null)
                    {
                        FileCreatedDeletedEventHandler(this, e);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
