using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BhabeshHuffmanEncoding.Interfaces
{
    public interface IGraphTraversal
    {
        IDictionary<char, string> GetHuffmannEncodingForCharacters(NodeData nodeData);
    }
}
