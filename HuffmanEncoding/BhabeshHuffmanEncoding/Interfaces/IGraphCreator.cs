using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BhabeshHuffmanEncoding.Interfaces
{
    public interface IGraphCreator
    {
        bool CreateGraph(IDictionary<char, int> uniqueCharacterToFrequencyMap);
        NodeData GetCreatedGraph();
    }
}
