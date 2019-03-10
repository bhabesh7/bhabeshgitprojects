using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMineLib.Model
{
    public class Cluster
    {
        public int RepresentativeIndex { get; set; }
        public List<int> AllIndexesInCluster { get; set; }

        public Cluster()
        {
            AllIndexesInCluster = new List<int>();
        }
    }
}
