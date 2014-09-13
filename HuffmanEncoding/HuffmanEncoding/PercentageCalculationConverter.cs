using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace HuffmanEncoding
{
    public class PercentageCalculationConverter:IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            float percentage = 0.0f;
            if (values.Count() == 2)
            {
                var valueString1 = values[0] as string;
                var valueString2 = values[1] as string;

                if (string.IsNullOrEmpty(valueString1) || string.IsNullOrEmpty(valueString2))
                {
                    return null;
                }
                int origBitCount = int.Parse(valueString1);
                int huffmannBitCount = int.Parse(valueString2);

                percentage = (huffmannBitCount *100)/ origBitCount;
                return (100 - percentage).ToString();
            }

            return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
