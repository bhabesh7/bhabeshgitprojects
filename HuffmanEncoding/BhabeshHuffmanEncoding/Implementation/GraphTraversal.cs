using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BhabeshHuffmanEncoding.Implementation;
using BhabeshHuffmanEncoding.Interfaces;

namespace BhabeshHuffmanEncoding.Implementation
{
    public class GraphTraversal:IGraphTraversal
    {
        IDictionary<char, string> _encodingDictionary = new Dictionary<char, string>();

        public IDictionary<char, string> GetHuffmannEncodingForCharacters(NodeData nodeData)
        {
            TraverseNode(nodeData, null);

            return _encodingDictionary;
        }

        void TraverseNode(NodeData nodeData, NodeData parentData)
        {
            if (parentData != null)
            {
                if (parentData.Connector != null)
                {
                    nodeData.Encoding = (parentData.Encoding == string.Empty) ? parentData.Connector.ToString() + nodeData.Connector.ToString() :
                        parentData.Encoding.ToString() + nodeData.Connector;
                }
                else
                {
                    nodeData.Encoding = nodeData.Connector.ToString();
                }
            }

            if ((nodeData.LeftNode == null) && (nodeData.RightNode == null))
            {
                var keyChar = Convert.ToChar(nodeData.Characters);
                if (!_encodingDictionary.ContainsKey(keyChar))
                {
                    nodeData.Encoding = parentData.Encoding + nodeData.Connector.ToString();
                    _encodingDictionary.Add(keyChar, nodeData.Encoding);
                }
            }

            if (!(nodeData.LeftNode == null))
            {
                TraverseNode(nodeData.LeftNode, nodeData);
            }
            if (!(nodeData.RightNode == null))
            {
                TraverseNode(nodeData.RightNode, nodeData);
            }
        }
    }
}
