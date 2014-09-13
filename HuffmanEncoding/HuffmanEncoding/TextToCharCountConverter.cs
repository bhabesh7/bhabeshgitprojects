
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace HuffmanEncoding
{
    public class TextToCharCountConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {   

            var content = value as string;

            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            return content.Trim().ToList().Count;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
