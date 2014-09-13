using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace HuffmanEncoding
{
    public class TextToBitCountConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var valueString = value as string;

            if (string.IsNullOrEmpty(valueString))
            {
                return null;
            }


            var charCount = int.Parse(valueString);

            if (charCount == null)
            {
                return null;
            }

            return charCount * 8;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
