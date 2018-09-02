using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneSearch.DataModel
{
    public class NameValuePair : BaseNotify
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("Name"));
            }
        }

        private string _value;

        public string Value
        {
            get { return _value; }
            set { _value = value;
                RaisePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("Value"));
            }
        }


    }
}
