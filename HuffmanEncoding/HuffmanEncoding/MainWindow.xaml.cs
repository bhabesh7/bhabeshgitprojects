using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BhabeshHuffmanEncoding.Implementation;
using BhabeshHuffmanEncoding.Interfaces;
using System.Collections.ObjectModel;

namespace HuffmanEncoding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IContentHandler _contentHandler = null;
        IGraphCreator _graphCreator = null;

        public ObservableCollection<DisplayData> _huffmannEncodedMap { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            _contentHandler = new TextContentHandler();
            _graphCreator = new GraphCreator();
            _huffmannEncodedMap = new ObservableCollection<DisplayData>();
            this.DataContext = _huffmannEncodedMap;
        }

        private void btnCompress_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text == string.Empty)
            {
                MessageBox.Show("Please enter Content");
                return;
            }

            _huffmannEncodedMap.Clear();
            var uniqueMap = _contentHandler.GetUniqueCharacterToFrequencyMapSortedByFreqInDescOrder(textBox1.Text);

            var result = _graphCreator.CreateGraph(uniqueMap);

            if (result)
            {
                var graph = _graphCreator.GetCreatedGraph();

                IGraphTraversal graphTraversal = new GraphTraversal();
                var dictionary = graphTraversal.GetHuffmannEncodingForCharacters(graph);

                foreach (var item in dictionary)
                {
                    _huffmannEncodedMap.Add(new DisplayData { Key = item.Key.ToString(), Value = item.Value });                    
                }
                listViewEncoding.DataContext = _huffmannEncodedMap;


                //get encoded bit data
                string message = string.Empty;
                var charList = textBox1.Text.ToCharArray().ToList();
                foreach (var item in charList)
                {
                    message += dictionary[item];
                }



                txbHuffmannCode.DataContext = new DisplayData { Key = message };

            }
        }
    }
}
