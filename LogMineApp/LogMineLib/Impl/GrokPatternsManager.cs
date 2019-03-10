using LogMineLib.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMineLib.Impl
{
    public class GrokPatternsManager : IGrokPatternsManager
    {
        JArray _patterns = new JArray();
        string _grokPatternsFilePath = @"Config\Patterns.mine";
        Dictionary<string, string> _grokPatternsDict = new Dictionary<string, string>();

        public IList<string> GetAllGrokKeys()
        {
            return _grokPatternsDict?.Keys?.ToList();
        }

        public string GetPattern(string GrokExpression)
        {
            string value = string.Empty;
            _grokPatternsDict.TryGetValue(GrokExpression, out value);
            return value;
        }

        public void LoadPatterns()
        {
            foreach (var item in File.ReadAllLines(_grokPatternsFilePath))
            {
                var results = item.Split(new char[] { '=' });
                if(results ==null || results.Length < 2)
                {
                    continue;
                }
                _grokPatternsDict.Add(results[0], results[1]);

            }
            //var jsonData = File.ReadAllText(jsonFilePath);

            //var allPatterns = JsonConvert.DeserializeObject(jsonData) as JArray;


            //foreach (JObject pattern in allPatterns)
            //{
            //    var grokKey = pattern.GetValue("key").Value<string>();                
            //    var grokVal = pattern.GetValue("value").Value<string>();
            //    _grokPatternsDict.Add(grokKey, grokVal);
            //}



        }
    }
}
