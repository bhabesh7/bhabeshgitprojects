using Accord.DataModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Accord.MainApp.Converters
{
    public class AnalysisStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AnalysisStatus)
            {
                var status = (AnalysisStatus)(value);

                switch (status)
                {
                    case AnalysisStatus.NotStarted:
                        return new SolidColorBrush(Colors.Yellow);

                    case AnalysisStatus.Completed_Ok:
                        return new SolidColorBrush(Colors.Green);

                    case AnalysisStatus.Completed_NG:
                        return new SolidColorBrush(Colors.Red);
                        
                    default:
                        break;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
