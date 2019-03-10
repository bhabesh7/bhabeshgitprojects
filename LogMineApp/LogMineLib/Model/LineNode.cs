using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMineLib.Model
{
    public class LineNode
    {
        public Cluster ClusterInfo { get; set; }
        public string PatternLine { get; set; }
        public LineData Line { get; set; }
        public int Level { get; set; }
        public LineNode LeftLineNode { get; set; }
        public LineNode RightLineNode { get; set; }
        public LineNode Parent { get; set; }

        public LineNode LeftSiblingNode { get; set; }
        public LineNode RightSiblingNode { get; set; }

        public LineNode()
        {
            ClusterInfo = new Cluster();
        }
    
    }

    public class LineData
    {
        public string Line { get; set; }        
        public int LineNum { get; set; }
    }

    public class MatchingPair
    {
        public int Index1 { get; set; }
        public int Index2 { get; set; }
    }
}
