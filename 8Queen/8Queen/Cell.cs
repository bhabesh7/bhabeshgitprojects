using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8Queen
{
    public class Cell : BaseModel
    {
        private int _value;

        public int Value
        {
            get { return _value; }
            set { _value = value; RaisePropertyChanged("Value"); }
        }
        
        
        
    }
}
