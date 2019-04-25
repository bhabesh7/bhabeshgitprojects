using Accord.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accord.Interfaces
{
    public interface IAnalysisManager
    {
        AnalysisResultData RunAnalysis(string programString);

    }
}
