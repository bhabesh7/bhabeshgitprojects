
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BhabeshHuffmanEncoding.Interfaces;

namespace BhabeshHuffmanEncoding.Implementation
{
    public class GraphCreator : IGraphCreator
    {
        IDictionary<char, int> _uniqueCharToFreqMapSortedInDesc;

        NodeData Graph = new NodeData();

        public GraphCreator()
        {
        }


        public bool CreateGraph(IDictionary<char, int> uniqueCharacterToFrequencyMap)
        {
            Graph = null;
            Graph = new NodeData();
            _uniqueCharToFreqMapSortedInDesc = uniqueCharacterToFrequencyMap;

            IList<NodeData> firstLevelNodes = new List<NodeData>();
            foreach (var item in _uniqueCharToFreqMapSortedInDesc)
            {
                var newLeafNode = CreateLeafNode(item.Key.ToString(), item.Value);
                //_characterGraph.AddFirst(newLeafNode);
                firstLevelNodes.Add(newLeafNode);
            }

            //var firstLevelNodes = _uniqueCharToFreqMapSortedInDesc.Select(x => x.Value).ToList();
            var isSuccess = TraverseBreadthAndCreateNewCombineNodes(firstLevelNodes);
            
            return isSuccess;
        }

        NodeData CreateLeafNode(string characters, int Frequency)
        {
            return new NodeData
            {
                Characters = characters,
                FrequencyOfOccurence = Frequency,
                IsTraversedDuringClassification = false,
                LeftNode = null,
                RightNode = null,
                Level = 0,
                Connector = null,
                Encoding = string.Empty
            };
        }


        bool TraverseBreadthAndCreateNewCombineNodes(IList<NodeData> nodeDataList)
        {
            var frequencies = nodeDataList.Select(x => x.FrequencyOfOccurence).OrderBy(y => y).ToList();
            //frequencies = from f in frequencies orderby f.frequency ascending select f;
            int currFreq = 0;
            int prevFreq = 0;
            bool isContinue = false;

            IList<NodeData> NodesOfNewLevel = new List<NodeData>();

            foreach (var freq in frequencies)
            {
                prevFreq = currFreq;
                currFreq = freq;

                var matched = nodeDataList.Where(x => x.FrequencyOfOccurence == freq && x.IsTraversedDuringClassification == false);

                if (ShouldContinueToNextItem(matched))
                {
                    continue;
                }

                int itemsCountMatchingCurrFreq = matched.Count();


                if (itemsCountMatchingCurrFreq == 1)
                {

                    var matchedItemFromCurrFreq = matched.SingleOrDefault();

                    if (matchedItemFromCurrFreq == null)
                    {
                        continue;
                    }

                    var matchedItemsFromPrevFreq = nodeDataList.Where(x => x.FrequencyOfOccurence == prevFreq && x.IsTraversedDuringClassification == false);

                    if (matchedItemsFromPrevFreq == null)
                    {
                        continue;
                    }

                    var itemMatchingPrevFreqButNotMatchingCurrFreq = matchedItemsFromPrevFreq.FirstOrDefault<NodeData>(x => x.Characters != matchedItemFromCurrFreq.Characters);

                    if (itemMatchingPrevFreqButNotMatchingCurrFreq == null)
                    {
                        continue;
                    }

                    var newNode = CreateCombinedNode(itemMatchingPrevFreqButNotMatchingCurrFreq, matchedItemFromCurrFreq);
                    NodesOfNewLevel.Add(newNode);
                }
                else
                {
                    var matchedItemFromCurrFreq = matched.FirstOrDefault();

                    var itemMatchingPrevFreqButNotMatchingCurrFreq = nodeDataList.FirstOrDefault(x => x.FrequencyOfOccurence == prevFreq && x.IsTraversedDuringClassification == false && x.Characters != matchedItemFromCurrFreq.Characters);

                    if (itemMatchingPrevFreqButNotMatchingCurrFreq != null && matchedItemFromCurrFreq != null)
                    {
                        var newNode = CreateCombinedNode(itemMatchingPrevFreqButNotMatchingCurrFreq, matchedItemFromCurrFreq);
                        NodesOfNewLevel.Add(newNode);
                    }
                    else
                    {
                        var twoMatchedItemsWithCurrentFreq = nodeDataList.Where(x => x.FrequencyOfOccurence == freq && x.IsTraversedDuringClassification == false).Take(2).ToList();
                        var newNode = CreateCombinedNode(twoMatchedItemsWithCurrentFreq[0], twoMatchedItemsWithCurrentFreq[1]);
                        NodesOfNewLevel.Add(newNode);
                    }
                }
            }

            var nodeNotPairedYet = nodeDataList.Where(x => x.IsTraversedDuringClassification == false).ToList();
            if (nodeNotPairedYet != null)
            {
                if (nodeNotPairedYet.Count() == 1)
                {
                    NodesOfNewLevel.Add(nodeNotPairedYet[0]);
                }
            }

            if (NodesOfNewLevel.Count > 1)
            {
                TraverseBreadthAndCreateNewCombineNodes(NodesOfNewLevel);
            }

            if (NodesOfNewLevel.Count == 1)
            {
                Graph = NodesOfNewLevel[0];
            }

            return true;
        }

        //static int _lambdaFreq = 0;
        //Func<NodeData, bool> _findMatchingFreqNonTraversedNodeFunc = (x => x.FrequencyOfOccurence == _lambdaFreq && x.IsTraversedDuringClassification == false);
               


        NodeData CreateCombinedNode(NodeData LeftNode, NodeData rightNode)
        {
            var combinedNodeData = new NodeData();

            combinedNodeData.Characters = LeftNode.Characters + rightNode.Characters;
            combinedNodeData.FrequencyOfOccurence = LeftNode.FrequencyOfOccurence + rightNode.FrequencyOfOccurence;
            combinedNodeData.IsTraversedDuringClassification = false;

            LeftNode.IsTraversedDuringClassification = true;
            rightNode.IsTraversedDuringClassification = true;

            combinedNodeData.LeftNode = LeftNode;
            combinedNodeData.LeftNode.Connector = 0;
            combinedNodeData.LeftNode.Encoding = string.Empty;
            combinedNodeData.RightNode = rightNode;
            combinedNodeData.RightNode.Connector = 1;
            combinedNodeData.RightNode.Encoding = string.Empty;
            combinedNodeData.Level = LeftNode.Level + 1; //or rightNode.Level+1
            
            return combinedNodeData;
        }

        private static bool ShouldContinueToNextItem(IEnumerable<NodeData> matched)
        {
            if (matched == null)
            {
                return true;
            }

            bool isContinue = false;
            switch (matched.Count())
            {
                case 0:
                    isContinue = true;
                    break;
                case 1:
                case 2:
                default:
                    isContinue = false;
                    break;
            }
            return isContinue;
        }

        
        public NodeData GetCreatedGraph()
        {
            Graph.Connector = null; //setting the Roots connector to NULL
            return Graph;
        }
    }
}
