using LuceneSearch.DataModel;
using LuceneSearch.Extensibility;
using LuceneSearch.Services.Impl;
using LuceneSearch.Services.Inte;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Linq;

namespace LuceneSearch
{
    public class MainViewModel : BaseNotify
    {
        SynchronizationContext _synchronizationContext = SynchronizationContext.Current;
        public ObservableCollection<NameValuePair> ConfigSettings { get; set; }
        public ObservableCollection<SearchFilterData> SearchFilterCollection { get; set; }

        private SearchFilterData _selectedFilterData;

        public SearchFilterData SelectedSearchFilter
        {
            get { return _selectedFilterData; }
            set
            {
                try
                {
                    if (SearchFilterCollection != null)
                    {
                        foreach (var item in SearchFilterCollection)
                        {
                            item.IsChecked = false;
                        }
                    }
                    _selectedFilterData = value;
                    _selectedFilterData.IsChecked = true;
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    RaisePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("SelectedSearchFilter"));
                    SearchCommand_Execute(new object());
                }
            }
        }


        //public ICommand ApplySearchSettingsCommand { get; set; }

        public ICommand ApplyIndexSettingsCommand { get; set; }


        private int _searchCount;
        public int SearchCount
        {
            get
            {
                return _searchCount;
            }
            set
            {
                _searchCount = value;
                RaisePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("SearchCount"));
            }
        }


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
            PopulateSearchFilterSettings();

            FetchAppConfigSettings();

            SearchCommand = new DelegateCommand(SearchCommand_CanExecute, SearchCommand_Execute);
            _searchManager.DocumentAddedEvent += _searchManager_DocumentAddedEvent;
            //ApplySearchSettingsCommand = new DelegateCommand((y) => { return true; }, (x) =>
            //{

            //    SearchCommand_Execute(new object());
            //});
            ApplyIndexSettingsCommand = new DelegateCommand((y) => { return true; }, (x) =>
            {
                SaveSettingsRebuildIndexRefresh();
            });

            BuildIndexCommand = new DelegateCommand((y) => { return true; }, (x) =>
            {
                BuildIndex();
            });
        }

        private void PopulateSearchFilterSettings()
        {
            SearchFilterCollection = new ObservableCollection<SearchFilterData>();

            try
            {
                XElement xElement = XElement.Load("SearchFiltersConfig.xml");
                var searchFilters = xElement.Elements();

                foreach (var filter in searchFilters)
                {
                    var nameAttr = filter.Attributes().FirstOrDefault((x) => x.Name == "Name");
                    if (nameAttr == null)
                    {
                        continue;
                    }
                    var name = nameAttr?.Value;

                    var valAttr = filter.Attributes().FirstOrDefault((x) => x.Name == "Value");
                    if (valAttr == null)
                    {
                        continue;
                    }
                    var value = valAttr?.Value;

                    SearchFilterCollection?.Add(new SearchFilterData(name, value, false));
                }

                //set the all filter to true
                SelectedSearchFilter = SearchFilterCollection?.FirstOrDefault();
                //allFilter.IsChecked = true;
            }
            catch (Exception ex)
            {
                CurrentStatus = ex.Message;
            }
            //SearchFilterCollection.Add(new SearchFilterData("ALL", ".all", true));
            //SearchFilterCollection.Add(new SearchFilterData("JPG", ".jpg", false));
            //SearchFilterCollection.Add(new SearchFilterData("PNG", ".png", false));
            //SearchFilterCollection.Add(new SearchFilterData("BMP", ".bmp", false));
            //SearchFilterCollection.Add(new SearchFilterData("GIF", ".gif", false));
            //SearchFilterCollection.Add(new SearchFilterData("XLS", ".xls", false));
            //SearchFilterCollection.Add(new SearchFilterData("XLSX", ".xlsx", false));
            //SearchFilterCollection.Add(new SearchFilterData("DOC", ".doc", false));
            //SearchFilterCollection.Add(new SearchFilterData("DOCX", ".docx", false));
            //SearchFilterCollection.Add(new SearchFilterData("PPT", ".ppt", false));
            //SearchFilterCollection.Add(new SearchFilterData("SLN", ".sln", false));
            //SearchFilterCollection.Add(new SearchFilterData("ODS", ".ods", false));
        }

        /// <summary>
        /// Build Index from stratch
        /// </summary>
        private void BuildIndex()
        {
            Task.Factory.StartNew(() =>
            {
                var dataLocation = ConfigurationManager.AppSettings.Get("DataLocation");
                var indexLocation = ConfigurationManager.AppSettings.Get("IndexLocation");

                Stopwatch sw = new Stopwatch();
                sw.Start();
                //_searchManager.IndexAddedEvent += _searchManager_IndexAddedEvent1; 
                //_searchManager.DocumentAddedEvent += _searchManager_DocumentAddedEvent;
                _searchManager.BuildIndex(new SearchContext { IndexPath = indexLocation, ScanPath = dataLocation });
                sw.Stop();
                Trace.WriteLine(string.Format("Time taken to build index {0}", sw.Elapsed.ToString()));
                //MessageBox.Show(string.Format("Time taken to re-build index {0}", sw.Elapsed.ToString()));

            }).ContinueWith((t) =>
            {
                SearchCommand_Execute(new object());
            }, TaskContinuationOptions.OnlyOnRanToCompletion).ContinueWith((t2) =>
            {
                MessageBox.Show("Settings saved, Index rebuilt, Search refreshed !!");
            });
        }

        /// <summary>
        /// Fetch App Config Settings
        /// </summary>
        private void FetchAppConfigSettings()
        {
            try
            {
                ConfigSettings = new ObservableCollection<NameValuePair>();

                ConfigSettings?.Clear();
                var keys = ConfigurationManager.AppSettings?.AllKeys;

                foreach (var item in keys)
                {
                    ConfigSettings.Add(new NameValuePair { Name = item, Value = ConfigurationManager.AppSettings[item] });
                }
            }
            catch (Exception ex)
            {
                CurrentStatus = ex.Message;
            }
        }

        /// <summary>
        /// Update changed section
        /// </summary>
        private void SaveSettingsRebuildIndexRefresh()
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                foreach (var item in ConfigSettings)
                {
                    config.AppSettings.Settings[item.Name].Value = item.Value;
                }

                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                BuildIndex();

            }
            catch (Exception ex)
            {

            }
        }

        private void _searchManager_DocumentAddedEvent(object sender, EventDataArgs e)
        {
            CurrentStatus = e?.Data;
        }

        private void SearchCommand_Execute(object obj)
        {
            //LucceneTest test = new LucceneTest();
            //test.BuildIndex();
            //var res = test.Search(SearchString);
            //MessageBox.Show(res);

            if (string.IsNullOrEmpty(SearchString))
            {
                return;
            }

            var documentDataList = _searchManager.Search(
                new SearchContext
                {
                    SearchString = SearchString,
                    IndexPath = ConfigurationManager.AppSettings["IndexLocation"],
                    ScanPath = ConfigurationManager.AppSettings["DataLocation"],
                    SearchFilterDataList = SearchFilterCollection?.ToList()
                });

            _synchronizationContext.Send((t) =>
            {
                SearchResultsCollection?.Clear();

                if (documentDataList != null)
                    SearchCount = documentDataList.Count;

                foreach (var docData in documentDataList)
                {
                    SearchResultsCollection.Add(docData);
                }
            }, null);

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
