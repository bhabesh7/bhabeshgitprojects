using LuceneSearch.DataModel;
using LuceneSearch.Extensibility;
using LuceneSearch.Services.Impl;
using LuceneSearch.Services.Inte;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace LuceneSearch
{
    public class MainViewModel : BaseNotify
    {
        Dispatcher _dispatcher = Dispatcher.CurrentDispatcher;

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

        public ObservableCollection<DocumentData> SearchResultsCollection { get; set; }


        private IFilesScanner _fileScanner = new FileScanner();
        private ISearchManager _searchManager = new LuceneSearchManager();

        public MainViewModel()
        {
            SearchResultsCollection = new ObservableCollection<DocumentData>();
            SearchString = string.Empty;
            SearchCommand = new DelegateCommand(SearchCommand_CanExecute, SearchCommand_Execute);

            //Task.Factory.StartNew(() =>
            //{
                var dataLocation = ConfigurationManager.AppSettings.Get("DataLocation");
                var indexLocation = ConfigurationManager.AppSettings.Get("IndexLocation");
                var documentDataList = _fileScanner.GetFileListWithFullPath(dataLocation);
                
                _searchManager.BuildIndex(new SearchContext { IndexPath = indexLocation, ScanPath = dataLocation }, 
                    documentDataList);
            //});
        }

        private void SearchCommand_Execute(object obj)
        {
            var documentDataList =_searchManager.Search(SearchString);
            SearchResultsCollection.Clear();

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
