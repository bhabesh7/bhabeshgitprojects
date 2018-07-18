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

namespace LuceneSearch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var model = (MainViewModel)this.DataContext;
            var txBox = sender as TextBox;
            model.SearchCommand.Execute(txBox.Text ==string.Empty ? "*:*": txBox.Text);
        }

        private void ContextMenu_Click(object sender, RoutedEventArgs e)
        {
            var model = (MainViewModel)this.DataContext;
            if (model == null)
            {
                return;
            }

            if (model.SelectedSearchResult == null)
            {
                return;
            }

            System.Diagnostics.Process.Start("explorer.exe",System.IO.Path.GetDirectoryName(model.SelectedSearchResult.FilePath));

        }
    }
}
