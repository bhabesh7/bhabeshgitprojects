using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Accord.MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void PieChart_DataClick(object sender, LiveCharts.ChartPoint chartpoint)
        {
            try
            {
                var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

                //clear selected slice.
                foreach (PieSeries series in chart.Series)
                    series.PushOut = 0;

                var selectedSeries = (PieSeries)chartpoint.SeriesView;
                selectedSeries.PushOut = 8;
                //selectedSeries.Name
            }
            catch (Exception)
            {                
            }
        }

    }
}

