using LogMineLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogMineLib.Model;
using System.Text.RegularExpressions;

namespace LogMineLib.Impl
{
    public class BasicLogTree : ILogTree
    {
        ILogReader _logReader;
        ILogAnalyzer _logAnalyzer;
        IGrokPatternsManager _patternsManager;
        IList<string> _logLines;


        public BasicLogTree()
        {
            _logReader = new BasicLogReader();
            _logAnalyzer = new LogAnalyzer();
            _patternsManager = new GrokPatternsManager();
            _logLines = new List<string>();
            _patternsManager.LoadPatterns();
        }
        /// <summary>
        /// Builds Tree and Returns 1 node
        /// </summary>
        /// <returns></returns>
        public IList<LineNode> BuildLogTree()
        {
            //LineNode firstNode = null;

            foreach (var line in _logReader.ReadLines(@"LogFiles\sample.log"))
            {
                _logLines.Add(line);
            }

            var clusters = _logAnalyzer.FormClusters(_logLines);
            IList<LineNode> rawLogClusters = new List<LineNode>();
            //Tree Representation
            foreach (var cluster in clusters)
            {
                LineNode newNode1 = GetLineNode(cluster.RepresentativeIndex + 1, 0, _logLines.ElementAt(cluster.RepresentativeIndex), cluster);
                newNode1.PatternLine = ConvertRawToPatternedString(cluster);

                rawLogClusters.Add(newNode1);
                //if (firstNode == null)
                //{
                //    firstNode = newNode1;
                //}
                //else
                //{
                //    firstNode.RightSiblingNode = newNode1;
                //    newNode1.LeftSiblingNode = firstNode;
                //}
            }

            #region old code
            //foreach (var item in clusters)
            //{
            //    //raw lines should always have 0 level (leaf node)
            //    LineNode newNode1 = GetLineNode(item.Index1, 0, _logLines.ElementAt(item.Index1));
            //    LineNode newNode2 = GetLineNode(item.Index2, 0, _logLines.ElementAt(item.Index2));

            //    string newLine1 = newNode1.Line.Line;
            //    string newLine2 = newNode2.Line.Line;

            //    foreach (var grokKey in _patternsManager.GetAllGrokKeys())
            //    {
            //        if (Regex.IsMatch(newLine1, _patternsManager.GetPattern(grokKey)))
            //        {
            //            newLine1 = Regex.Replace(newLine1, _patternsManager.GetPattern(grokKey), grokKey);
            //        }

            //        if (Regex.IsMatch(newLine2, _patternsManager.GetPattern(grokKey)))
            //        {
            //            newLine2 = Regex.Replace(newLine2, _patternsManager.GetPattern(grokKey), grokKey);
            //        }
            //    }

            //    LineNode parentNode = GetLineNode(-1, 1, newLine1);
            //    parentNode.LeftLineNode = newNode1;
            //    parentNode.RightLineNode = newNode2;

            //    firstNode = parentNode;
            //}
            #endregion old code

            return rawLogClusters;
        }

        public IList<string> GetAllLogLines()
        {
            return _logLines;
        }

        private string ConvertRawToPatternedString(Cluster cluster)
        {
            string logMsgForPatterning = _logLines.ElementAt(cluster.RepresentativeIndex);
            foreach (var grokKey in _patternsManager.GetAllGrokKeys())
            {
                logMsgForPatterning = Regex.Replace(logMsgForPatterning, _patternsManager.GetPattern(grokKey), grokKey);
            }

            return logMsgForPatterning;
        }

        private LineNode GetLineNode(int lineNum, int level, string data, Cluster clusterInfo)
        {
            return new LineNode
            {
                ClusterInfo = clusterInfo,
                LeftLineNode = null,
                RightLineNode = null,
                Level = level,
                Line = new LineData
                {
                    LineNum = lineNum,
                    Line = data
                },
            };
        }


    }
}
