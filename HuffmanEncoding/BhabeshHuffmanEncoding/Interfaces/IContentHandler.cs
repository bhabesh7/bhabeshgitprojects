using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BhabeshHuffmanEncoding.Interfaces
{
    public interface IContentHandler
    {
        IDictionary<char, int> GetUniqueCharacterToFrequencyMapSortedByFreqInDescOrder(string content);
    }
}
