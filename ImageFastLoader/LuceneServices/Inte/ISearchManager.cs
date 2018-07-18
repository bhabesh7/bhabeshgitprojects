using ImageFastLoader.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageFastLoader.LuceneServices.Inte
{
    public interface ISearchManager
    {
        bool BuildIndex(SearchContext context, IList<DocumentData> documentDataList);
        IList<DocumentData> Search(SearchContext searchContext);
        
    }
}
