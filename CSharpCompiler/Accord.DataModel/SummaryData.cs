using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accord.DataModel
{
    public class SummaryData : BindableBase
    {

        private string _summaryName;

        public string SummaryName
        {
            get { return _summaryName; }
            set { _summaryName = value; RaisePropertyChanged(nameof(SummaryName)); }
        }

        private int _count;

        public int SummaryCount
        {
            get { return _count; }
            set
            {
                _count = value;
                RaisePropertyChanged(nameof(SummaryCount));
            }
        }


    }
}
