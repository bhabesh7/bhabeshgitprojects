using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageFastLoader
{
    public class Picture:BaseNotify
    {
        private BitmapImage myVar;

        public BitmapImage Pic
        {
            get { return myVar; }
            set
            {
                myVar = value;
                RaisePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("Pic"));
            }
        }
    }
}
