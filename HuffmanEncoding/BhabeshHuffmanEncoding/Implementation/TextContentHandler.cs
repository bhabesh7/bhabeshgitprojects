using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BhabeshHuffmanEncoding.Interfaces;

namespace BhabeshHuffmanEncoding.Implementation
{
    public class TextContentHandler : IContentHandler
    {
        IList<char> _originalCharacterListFromContent = new List<char>();
        IList<char> _asciiSupportedList = new List<char>();

        private void Initialize(string content)
        {
            _originalCharacterListFromContent.Clear();
            _originalCharacterListFromContent = content.ToCharArray().ToList();
            PopulateAsciiSupportedList();
        }

        private void PopulateAsciiSupportedList()
        {
            _asciiSupportedList.Clear();
            for (int asciiChar = 32; asciiChar < 127; asciiChar++)
            {
                _asciiSupportedList.Add((char)asciiChar);
            }
        }


        public IDictionary<char, int> GetUniqueCharacterToFrequencyMapSortedByFreqInDescOrder(string content)
        {
            Initialize(content);
            IDictionary<char, int> uniqueCharacterToFrequencyMap = new Dictionary<char, int>();
            foreach (char supportedChar in _asciiSupportedList)
            {
                var matchedChars = _originalCharacterListFromContent.Where(x => x == supportedChar);

                if (matchedChars == null)
                {
                    continue;
                }

                if (matchedChars.Count() == 0)
                {
                    continue;
                }

                uniqueCharacterToFrequencyMap.Add(supportedChar, matchedChars.Count());
            }

            var sorted = from pair in uniqueCharacterToFrequencyMap orderby pair.Value descending select pair;

            return sorted.ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
