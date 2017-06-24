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
using System.Windows.Threading;

namespace ImageFastLoader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            tbMemory.Text = string.Format("{0:0.00} MB", GC.GetTotalMemory(true) / 1024.0 / 1024.0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // create the demo items provider according to specified parameters
            int numItems = int.Parse(tbNumItems.Text);
            int fetchDelay = int.Parse(tbFetchDelay.Text);
            ImageProvider imageProvider = new ImageProvider(numItems, fetchDelay);

            // create the collection according to specified parameters
            int pageSize = int.Parse(tbPageSize.Text);
            int pageTimeout = int.Parse(tbPageTimeout.Text);

            if (rbNormal.IsChecked.Value)
            {
                DataContext = new List<Picture>(imageProvider.FetchRange(0, imageProvider.FetchCount()));
            }
            else if (rbVirtualizing.IsChecked.Value)
            {
                DataContext = new VirtualizingCollection<Picture>(imageProvider, pageSize);
            }
            else if (rbAsync.IsChecked.Value)
            {
                DataContext = new AsyncVirtualizingCollection<Picture>(imageProvider, pageSize, pageTimeout * 1000);
            }
        }
    }
}
