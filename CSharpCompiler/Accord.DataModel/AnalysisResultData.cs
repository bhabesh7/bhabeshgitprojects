using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accord.DataModel
{
    public class AnalysisResultData
    {
        public ObservableCollection<NameRuleError> NameRuleErrors { get; set; }

        public string GeneratedInterfaceString { get; set; }

        public AnalysisResultData()
        {
            NameRuleErrors = new ObservableCollection<NameRuleError>();
            GeneratedInterfaceString = string.Empty;
        }
    }
}
