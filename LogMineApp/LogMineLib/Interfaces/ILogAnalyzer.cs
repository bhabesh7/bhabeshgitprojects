using LogMineLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMineLib.Interfaces
{
    internal interface ILogAnalyzer
    {
        /// <summary>
        /// Compares and returns a score (distance) 
        /// </summary>
        /// <param name="logMsg1"></param>
        /// <param name="logStr2"></param>
        /// <returns></returns>
        float Compare(string logMsg1, string logStr2);

        /// <summary>
        /// Clusters on the basis of score calculation of log distances
        /// </summary>
        /// <param name="logLines"></param>
        /// <returns></returns>
        IList<MatchingPair> ClusterRawLines(IList<string> logLines);

        IList<Cluster> FormClusters(IList<string> logLines);

        
    }
}
