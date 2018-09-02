﻿using LuceneSearch.DataModel;
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

namespace LuceneSearch
{
    public class MainViewModel : BaseNotify
    {
        SynchronizationContext _synchronizationContext = SynchronizationContext.Current;
        public ObservableCollection<NameValuePair> ConfigSettings { get; set; }



        public ICommand SaveSettingsCommand { get; set; }


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
            SearchCommand = new DelegateCommand(SearchCommand_CanExecute, SearchCommand_Execute);
            _searchManager.DocumentAddedEvent += _searchManager_DocumentAddedEvent;
            ConfigSettings = new ObservableCollection<NameValuePair>();
            FetchAppConfigSettings();
            SaveSettingsCommand = new DelegateCommand((y) => { return true; }, (x) =>
             {
                 SaveSettingsRebuildIndexRefresh();
             });

            BuildIndexCommand = new DelegateCommand((y) => { return true; }, (x) =>
            {
                BuildIndex();
            });
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

            var documentDataList = _searchManager.Search(SearchString);

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
