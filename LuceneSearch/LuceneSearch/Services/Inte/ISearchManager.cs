using LuceneSearch.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneSearch.Services.Inte
{
    public class EventDataArgs : EventArgs
    {
        public string Data;
    }

    public interface ISearchManager
    {
        //public delegate void IndexAddedDelegate(string doc);
        //event IndexAddedDelegate IndexAddedEvent;
        event EventHandler<EventDataArgs> DocumentAddedEvent;
        bool BuildIndex(SearchContext context);
        //IList<DocumentData> Search(string searchString);
        IList<DocumentData> Search(SearchContext searchContext);
        bool AddDocumentToIndex(string fullFilePath, SearchContext context);
        
    }
}
