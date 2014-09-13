using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuffmanAlgorithm
{
    public class MapData
    {
        public int Value { get; set; }
        public string Key { get; set; }

        public MapData LeftNode { get; set; }
        public MapData Rightode { get; set; }
    }
}
