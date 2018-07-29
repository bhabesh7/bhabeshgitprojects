using LuceneSearch.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneSearch.Services.Inte
{
    public interface IFilesScanner
    {
        IEnumerable<DocumentData> GetFileListWithFullPath(string location);
        event EventHandler<FileSystemEventArgs> FileCreatedDeletedEventHandler;
        event EventHandler<RenamedEventArgs> FileRenamedEventHandler;
        void SetupFileWatcher();
    }
}
