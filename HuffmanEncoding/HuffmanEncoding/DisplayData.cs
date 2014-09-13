using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace HuffmanEncoding
{
    public class DisplayData:INotifyPropertyChanged
    {
        private string key;
        public string Key { get {return key;} 
            set {
                key = value;
                OnPropertyChanged("Key");
            } 
        }
        private string value1 { get; set; }

        public string Value
        {
            get { return value1; }
            set
            {
                value1 = value;
                //PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                OnPropertyChanged("Value");
            }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged!=null)
            {
                PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
