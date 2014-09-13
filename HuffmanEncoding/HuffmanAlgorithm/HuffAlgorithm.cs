using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuffmanAlgorithm
{
    public class HuffAlgorithm
    {

        string _data = string.Empty;
        int _lowest = -1;
        //IDictionary<string, int> _distinctCharacterMap = new Dictionary<string, int>();
        IList<MapData> _collectedList = new List<MapData>();
        

        public void ProcessText(string data)
        {
            _data = data;
            ParseAndFillMapFirstTime();
            PickTheTwoLeastFoundElementsAndRecurse();
        }

        void ParseAndFillMapFirstTime()
        {           
           
            foreach (var character in _data)
            {
                var valueString = character.ToString();

                if (_collectedList.SingleOrDefault(x => x.Key == valueString) == null)
                {
                    _collectedList.Add(new MapData { Key = valueString, Value = 1 });
                }
                else
                {
                    _collectedList.Single(x => x.Key == valueString).Value += 1;

                }                              
            }

            SortCollectedList();
        }

        void SortCollectedList()
        {
            var sorted = _collectedList.OrderByDescending(x => x.Value);
            _collectedList = null;
            _collectedList = sorted.Cast<MapData>().ToList();
            _lowest = _collectedList.Last().Value;
        }

        void PickTheTwoLeastFoundElementsAndRecurse()
        {
            string newKey =string.Empty;
            int newValue = -1;

            if (_collectedList.Count == 2)
            {
                newKey = _collectedList[0].Key + _collectedList[1].Key;
                newValue = _collectedList[0].Value + _collectedList[1].Value;
                
                _collectedList.Add(new MapData { Key = newKey, Value = newValue, LeftNode = _collectedList[0], Rightode = _collectedList[1] });
                //_collectedList.Remove(_collectedList[0]);
                //_collectedList.Remove(_collectedList[1]);
                return;
            }


            var items = _collectedList.Where(x => x.Value == _lowest).ToList<MapData>();
            
            var count = items.Count();

            if(count >=2)
            {
                newKey = items[0].Key + items[1].Key;
                newValue = items[0].Value + items[1].Value;
                
                _collectedList.Add(new MapData { Key = newKey, Value = newValue, LeftNode = items[0], Rightode = items[1] });
                _collectedList.Remove(items[0]);
                _collectedList.Remove(items[1]);
                _lowest = GetNewLowest();
            }
            else if(count == 1)
            {
                int minDiff = -1;// _collectedList[0].Value - _lowest;
                int indexOfMinDiff = -1;
                var newLowest = GetNewLowest();

                var newLowestKVP = _collectedList.Single(x => x.Value == newLowest);
                newKey = items[0].Key + newLowestKVP.Key;
                newValue = items[0].Value + newLowestKVP.Value;

                _collectedList.Add(new MapData { Key = newKey, Value = newValue, LeftNode = items[0], Rightode = newLowestKVP });
                _collectedList.Remove(items[0]);
                _collectedList.Remove(newLowestKVP);
                _lowest = GetNewLowest();
            }
            

            if (_collectedList.Count != 1)
            {
                PickTheTwoLeastFoundElementsAndRecurse();
            }
        }

        private int GetNewLowest()
        {
            //<diff,index>
            IList<int> diffList = new List<int>();
            for (int i = 0; i < _collectedList.Count; i++)
            {
                //if (diffList.ContainsKey(_collectedList[i].Value - _lowest) == false)
                //{
                //    diffList.Add(_collectedList[i].Value - _lowest, i);
                //}
                
                //if (diffList.Where(x=> x == (_collectedList[i].Value - _lowest)).Count() == 0)
                {
                    diffList.Add(_collectedList[i].Value - _lowest);
                }
            }
            var sortedDiffList = diffList.OrderByDescending(x => x);

            diffList = null;
            diffList = sortedDiffList.Cast<int>().ToList();
           

            if (diffList.Count > 2)
            {
                diffList.RemoveAt(diffList.Count - 1);
            }
            //new lowest
            var newLowest = diffList.Last() + _lowest;
            return newLowest;
        }

    }
}
