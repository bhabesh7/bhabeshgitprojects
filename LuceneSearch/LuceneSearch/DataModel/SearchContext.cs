using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneSearch.DataModel
{
    public class SearchContext
    {
        public string IndexPath { get; set; }
        public string ScanPath { get; set; }

        public IList<SearchFilterData> SearchFilterDataList { get; set; }

        public string SearchString { get; set; }

        public SearchContext()
        {
            IndexPath = string.Empty;
            ScanPath = string.Empty;
            SearchFilterDataList = new List<SearchFilterData>();
            SearchString = string.Empty;
        }

    }
}
