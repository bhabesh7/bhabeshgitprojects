using LuceneSearch.DataModel;
using LuceneSearch.Extensibility;
using LuceneSearch.Services.Impl;
using LuceneSearch.Services.Inte;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace LuceneSearch
{
    public class MainViewModel : BaseNotify
    {
        Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

        private string _currentStatus;
        public string CurrentStatus
        {
            get
            {
                return _currentStatus;
            }
            set
            {
                _currentStatus = value;
                RaisePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("CurrentStatus"));
            }
        }

        private string _searchString;

        public string SearchString
        {
            get { return _searchString; }
            set
            {
                _searchString = value;
                RaisePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("SearchString"));
            }
        }

        private ICommand _buildIndexCommand;

        public ICommand BuildIndexCommand
        {
            get { return _buildIndexCommand; }
            set
            {
                _buildIndexCommand = value;
                RaisePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("SearchString"));
            }
        }

        public ObservableCollection<DocumentData> SearchResultsCollection { get; set; }

        public DocumentData _selectedSearchResult;
        public DocumentData SelectedSearchResult
        {
            get
            {
                return _selectedSearchResult;
            }
            set
            {
                _selectedSearchResult = value;
                RaisePropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("SelectedSearchResult"));
            }
        }

        private IFilesScanner _fileScanner = new FileScanner();
        private ISearchManager _searchManager = new LuceneSearchManager();

        public MainViewModel()
        {
            SearchResultsCollection = new ObservableCollection<DocumentData>();
            SearchString = "*:*";
            SearchCommand = new DelegateCommand(SearchCommand_CanExecute, SearchCommand_Execute);
            BuildIndexCommand = new DelegateCommand((y)=> { return true; },(x) =>
            {
                Task.Factory.StartNew(() =>
                {
                    var dataLocation = ConfigurationManager.AppSettings.Get("DataLocation");
                    var indexLocation = ConfigurationManager.AppSettings.Get("IndexLocation");
                    
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    //_searchManager.IndexAddedEvent += _searchManager_IndexAddedEvent1; ;
                    _searchManager.BuildIndex(new SearchContext { IndexPath = indexLocation, ScanPath = dataLocation });
                    sw.Stop();
                    Trace.WriteLine( string.Format("Time taken to build index {0}", sw.Elapsed.ToString()));
                    MessageBox.Show(string.Format("Index built. Time taken to build index {0}", sw.Elapsed.ToString()));
                });
            });
        }

       

        private void SearchCommand_Execute(object obj)
        {
            //LucceneTest test = new LucceneTest();
            //test.BuildIndex();
            //var res = test.Search(SearchString);
            //MessageBox.Show(res);

            if(string.IsNullOrEmpty(SearchString))
            {
                return;
            }

            var documentDataList = _searchManager.Search(SearchString);
            SearchResultsCollection?.Clear();

            foreach (var docData in documentDataList)
            {
                SearchResultsCollection.Add(docData);
            }
        }

        private bool SearchCommand_CanExecute(object arg)
        {
            return true;
        }

        private ICommand _searchCommand;

        public ICommand SearchCommand
        {
            get { return _searchCommand; }
            set
            {
                _searchCommand = value;
                RaisePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("SearchCommand"));
            }
        }


    }
}
