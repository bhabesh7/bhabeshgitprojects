using LuceneSearch.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneSearch.Services.Inte
{
    public interface IFilesScanner
    {
        IList<DocumentData> GetFileListWithFullPath(string location);
    }
}
