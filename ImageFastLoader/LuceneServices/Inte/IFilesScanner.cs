using ImageFastLoader.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageFastLoader.LuceneServices.Inte
{
    public interface IFilesScanner
    {
        IList<DocumentData> GetFileListWithFullPath(string location);
    }
}
