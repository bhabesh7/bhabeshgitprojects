using LogMineLib.Interfaces;
using LogMineLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMineLib.Impl
{
    internal class LogAnalyzer : ILogAnalyzer
    {
        const float MAX_RAW_LOG_MSG_DIST = 1.0f;

        public float Compare(string logStr1, string logStr2)
        {
            char[] sep = { ' ' };
            if (string.IsNullOrEmpty(logStr1) || string.IsNullOrEmpty(logStr2))
            {
                return -1;
            }

            var fields1Array = logStr1.Split(sep);
            var fields2Array = logStr2.Split(sep);

            var field1Len = fields1Array.Length;
            var field2Len = fields2Array.Length;

            var scoreAtIndexes = new List<float>();

            float score = 0.0f;
            if (field1Len < field2Len)
            {
                score = GetRawDistanceScore(fields1Array, fields2Array);
            }
            else if (field2Len < field1Len)
            {
                score = GetRawDistanceScore(fields2Array, fields1Array);
            }
            else
            {
                //lengths are equal
                score = GetRawDistanceScore(fields2Array, fields1Array);
            }


            return score;
        }

        //private float Add(float x, float y)
        //{
        //    return x + y;
        //}

        /// <summary>
        /// GetDistance score for 2 raw log lines. Returns -1 
        /// </summary>
        /// <param name="smallerArray">field representation of log line 1</param>
        /// <param name="biggerArray">field representation of log line 2</param>
        /// <returns></returns>
        private float GetRawDistanceScore(string[] smallerArray, string[] biggerArray)
        {
            float score = 0.0f;

            for (int i = 0; i < smallerArray.Length; i++)
            {
                if (smallerArray[i] == biggerArray[i])
                {
                    score += (float)(1.0f / (biggerArray.Length));
                }

                //Early Abandon field score calc if Max score TH is already reached.
                //Higher score indictes less variation between the log lines being compared.
                //if (score >= MAX_RAW_LOG_MSG_DIST)
                //{
                //    return -1;
                //}
            }

            //distance = 1 - similarity
            return (10.0f - score);
        }

        /// <summary>
        /// Makes clusters out of log msg's which match distance criteria
        /// </summary>
        /// <param name="logLines"></param>
        /// <returns>A collection of clusters</returns>
        public IList<Cluster> FormClusters(IList<string> logLines)
        {
            IList<Cluster> clusterList = new List<Cluster>();
            IList<int> clusteredStateList = logLines.Select((x) => -1).ToList();

            float minInterLogDist = 10f; //impossibly high to reach, that's good !
                        
            for (int i = 0; i < logLines.Count; i++)
            {
                int tempMatchIndex = 0;
                minInterLogDist = 10f;
                //bool isClustered = IsLogMsgClustered(clusterList, i);                
                if (clusteredStateList[i] != -1)
                {
                    //item is already clustered , so continue looking
                    continue;
                }

                if (clusteredStateList[i] == -1)
                {                    
                    clusteredStateList[i] = i;
                    CreateNewCluster(clusterList, i);
                }

                for (int j = i + 1; j < logLines.Count; j++)
                {
                    //no point processing an already clustered log Msg
                    if(clusteredStateList[j] != -1)
                    {
                        continue;
                    }

                    var result = Compare(logLines[i], logLines[j]);

                    if (result < minInterLogDist && result >= 0)
                    {
                        //if dynamically updated , then it makes more clusters.
                        //If minInterLogDist is constant then all converge to 1 cluster (unless difference is huge) 
                        minInterLogDist = result;
                        tempMatchIndex = j;
                        //add index in existing cluster
                        var cluster = clusterList.FirstOrDefault((x) => x.RepresentativeIndex == i);
                        cluster.AllIndexesInCluster.Add(tempMatchIndex);
                        //Update cluster representative index at logIndex
                        clusteredStateList[tempMatchIndex] = i;
                    }
                }               
                
            }

            return clusterList;

        }

        private void CreateNewCluster(IList<Cluster> clusterList, int i)
        {
            var cluster = new Cluster { RepresentativeIndex = i };
            cluster.AllIndexesInCluster.Add(i);
            clusterList.Add(cluster);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clusterList"></param>
        /// <param name="logIndex"></param>
        /// <returns></returns>
        private bool IsLogMsgClustered(IList<Cluster> clusterList, int logIndex)
        {
            var cluster = clusterList?.FirstOrDefault((x) => x.AllIndexesInCluster.Contains(logIndex));
            return (cluster != null);
        }



        /// <summary>
        /// ClusterRawLines
        /// </summary>
        public IList<MatchingPair> ClusterRawLines(IList<string> logLines)
        {
            IList<MatchingPair> matchingPairList = new List<MatchingPair>();
            IList<bool> clusteredStateList = logLines.Select((x) => false).ToList();

            float minInterLogDist = 10f; //impossibly high to reach, that's good !

            for (int i = 0; i < logLines.Count; i++)
            {
                int tempMatchIndex = 0;
                minInterLogDist = 10f;

                for (int j = i + 1; j < logLines.Count; j++)
                {
                    if (clusteredStateList[i] || clusteredStateList[j])
                    {
                        continue;
                    }

                    var result = Compare(logLines[i], logLines[j]);

                    if (result < minInterLogDist && result >= 0)
                    {
                        minInterLogDist = result;
                        tempMatchIndex = j;
                    }
                }
                //The pair that got nearest match and are clubbed        
                if (clusteredStateList[i] || clusteredStateList[tempMatchIndex])
                {
                    continue;
                }
                matchingPairList.Add(new MatchingPair { Index1 = i, Index2 = tempMatchIndex });
                clusteredStateList[i] = true;
                clusteredStateList[tempMatchIndex] = true;
            }

            return matchingPairList;
        }

      
    }
}
