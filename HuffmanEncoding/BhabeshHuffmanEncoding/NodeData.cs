using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BhabeshHuffmanEncoding
{
    public class NodeData
    {
        public NodeData LeftNode { get; set; }
        public NodeData RightNode { get; set; }
        public int FrequencyOfOccurence { get; set; }
        public string Characters { get; set; }
        public int Level { get; set; }
        public bool IsTraversedDuringClassification { get; set; }
        public int? Connector { get; set; }
        public string Encoding { get; set; }
        //public bool? RightBit { get; set; }        
    }
}
