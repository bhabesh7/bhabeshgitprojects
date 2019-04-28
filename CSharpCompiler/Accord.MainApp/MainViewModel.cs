using Accord.DataModel;
using Accord.Interfaces;
using LiveCharts;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Accord.MainApp
{
    public class MainViewModel : BindableBase
    {
        IFolderScanner _folderScanner;
        ICodeParser _codeParser;
        IAnalysisManager _analysisManager;

        public Func<ChartPoint, string> PointLabel { get; set; }



        private ICommand _violationSummaryCommand;

        public ICommand ViolationSummaryCommand
        {
            get { return _violationSummaryCommand; }
            set
            {
                _violationSummaryCommand = value;
                RaisePropertyChanged(nameof(ViolationSummaryCommand));
            }
        }


        private ICommand analyzeCommand;

        public ICommand AnalyzeCommand
        {
            get { return analyzeCommand; }
            set { analyzeCommand = value; RaisePropertyChanged(nameof(AnalyzeCommand)); }
        }

        private ICommand _loadCodeCommand;

        public ICommand LoadCodeCommand
        {
            get { return _loadCodeCommand; }
            set { _loadCodeCommand = value; RaisePropertyChanged(nameof(LoadCodeCommand)); }
        }

        private string _origCodeString;

        public string OrigCodeString
        {
            get
            {
                return _origCodeString;
            }
            set { _origCodeString = value; RaisePropertyChanged(nameof(OrigCodeString)); }
        }

        private string _codeRootLocation;
        public string CodeRootLocation
        {
            get { return _codeRootLocation; }
            set
            {
                _codeRootLocation = value;
                RaisePropertyChanged(nameof(CodeRootLocation));
            }
        }

        private string inputText;

        public string InputText
        {
            get { return inputText; }
            set { inputText = value; RaisePropertyChanged(nameof(inputText)); }
        }

        private bool isResultsExpanded;

        public bool IsResultExpanded
        {
            get { return isResultsExpanded; }
            set { isResultsExpanded = value; RaisePropertyChanged(nameof(IsResultExpanded)); }
        }

        private DocumentData _selectedSourceCodeFile;

        public DocumentData SelectedSourceCodeFile
        {
            get { return _selectedSourceCodeFile; }
            set
            {
                _selectedSourceCodeFile = value;
                IsResultExpanded = true;
                if (_selectedSourceCodeFile != null)
                {
                    OrigCodeString = _codeParser.GetCodeFromFile(_selectedSourceCodeFile.FilePath);
                }
                else
                {
                    OrigCodeString = string.Empty;
                }
                RaisePropertyChanged(nameof(SelectedSourceCodeFile));
            }
        }



        public ObservableCollection<DocumentData> SourceCodeFilesCollection { get; set; }

        public ObservableCollection<SummaryData> ViolationSummaryCollection { get; set; }

        #region PieChart Properties
        private ChartValues<int> _namespaceNameViolationsChartValue;

        public ChartValues<int> NamespaceNameViolationsChartValue
        {
            get { return _namespaceNameViolationsChartValue; }
            set
            {
                _namespaceNameViolationsChartValue = value;
                RaisePropertyChanged(nameof(NamespaceNameViolationsChartValue));
            }
        }

        private ChartValues<int> _classNameViolationsChartValue;

        public ChartValues<int> ClassNameViolationsChartValue
        {
            get { return _classNameViolationsChartValue; }
            set { _classNameViolationsChartValue = value; RaisePropertyChanged(nameof(ClassNameViolationsChartValue)); }
        }

        private ChartValues<int> _methodNameViolationsChartValue;

        public ChartValues<int> MethodNameViolationsChartValue
        {
            get { return _methodNameViolationsChartValue; }
            set { _methodNameViolationsChartValue = value; RaisePropertyChanged(nameof(MethodNameViolationsChartValue)); }
        }

        private ChartValues<int> _parameterNameViolationsChartValue;

        public ChartValues<int> ParameterNameViolationsChartValue
        {
            get { return _parameterNameViolationsChartValue; }
            set { _parameterNameViolationsChartValue = value; RaisePropertyChanged(nameof(ParameterNameViolationsChartValue)); }
        }


        private ChartValues<int> _privatePropertyNameViolationsChartValue;

        public ChartValues<int> PrivatePropertyNameViolationsChartValue
        {
            get { return _privatePropertyNameViolationsChartValue; }
            set
            {
                _privatePropertyNameViolationsChartValue = value;
                RaisePropertyChanged(nameof(PrivatePropertyNameViolationsChartValue));
            }
        }

        private ChartValues<int> _protectedPropertyNameViolationsChartValue;

        public ChartValues<int> ProtectedPropertyNameViolationsChartValue
        {
            get { return _protectedPropertyNameViolationsChartValue; }
            set
            {
                _protectedPropertyNameViolationsChartValue = value;
                RaisePropertyChanged(nameof(ProtectedPropertyNameViolationsChartValue));
            }
        }



        private ChartValues<int> _publicPropertyNameViolationsChartValue;

        public ChartValues<int> PublicPropertyNameViolationsChartValue
        {
            get { return _publicPropertyNameViolationsChartValue; }
            set
            {
                _publicPropertyNameViolationsChartValue = value;
                RaisePropertyChanged(nameof(PublicPropertyNameViolationsChartValue));
            }
        }

        private ChartValues<int> _privateFieldNameViolationsChartValue;

        public ChartValues<int> PrivateFieldNameViolationsChartValue
        {
            get { return _privateFieldNameViolationsChartValue; }
            set
            {
                _privateFieldNameViolationsChartValue = value;
                RaisePropertyChanged(nameof(PrivateFieldNameViolationsChartValue));
            }
        }

        private ChartValues<int> _protectedFieldNameViolationsChartValue;

        public ChartValues<int> ProtectedFieldNameViolationsChartValue
        {
            get { return _protectedFieldNameViolationsChartValue; }
            set
            {
                _protectedFieldNameViolationsChartValue = value;
                RaisePropertyChanged(nameof(ProtectedFieldNameViolationsChartValue));
            }
        }

        private ChartValues<int> _publicFieldNameViolationsChartValue;

        public ChartValues<int> PublicFieldNameViolationsChartValue
        {
            get { return _publicFieldNameViolationsChartValue; }
            set
            {
                _publicFieldNameViolationsChartValue = value;
                RaisePropertyChanged(nameof(PublicFieldNameViolationsChartValue));
            }
        }

        private ChartValues<int> _largeMethodViolationsChartValue;

        public ChartValues<int> LargeMethodViolationsChartValue
        {
            get { return _largeMethodViolationsChartValue; }
            set
            {
                _largeMethodViolationsChartValue = value;
                RaisePropertyChanged(nameof(LargeMethodViolationsChartValue));
            }
        }

        private int _totalViolationsCount;

        public int TotalViolationsCount
        {
            get { return _totalViolationsCount; }
            set
            {
                _totalViolationsCount = value;
                RaisePropertyChanged(nameof(TotalViolationsCount));
            }
        }


        #endregion Piechart properties


        public MainViewModel(IFolderScanner folderScanner, ICodeParser codeParser, IAnalysisManager analysisManager)
        {
            _folderScanner = folderScanner;
            _codeParser = codeParser;
            _analysisManager = analysisManager;
            SourceCodeFilesCollection = new ObservableCollection<DocumentData>();
            ViolationSummaryCollection = new ObservableCollection<SummaryData>();
            CodeRootLocation = @"b:\samplefile";
            InitializeCommands();
            PointLabel = chartPoint =>
               string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            InitializePieChartProperties();
        }

        private void InitializePieChartProperties()
        {
            NamespaceNameViolationsChartValue = new ChartValues<int> { 0 };
            ClassNameViolationsChartValue = new ChartValues<int> { 0 };
            MethodNameViolationsChartValue = new ChartValues<int> { 0 };
            ParameterNameViolationsChartValue = new ChartValues<int> { 0 };

            PublicPropertyNameViolationsChartValue = new ChartValues<int> { 0 };
            ProtectedPropertyNameViolationsChartValue = new ChartValues<int> { 0 };
            PrivatePropertyNameViolationsChartValue = new ChartValues<int> { 0 };

            PublicFieldNameViolationsChartValue = new ChartValues<int> { 0 };
            ProtectedFieldNameViolationsChartValue = new ChartValues<int> { 0 };
            PrivateFieldNameViolationsChartValue = new ChartValues<int> { 0 };
            LargeMethodViolationsChartValue = new ChartValues<int> { 0 };
            TotalViolationsCount = 0;
        }

        private void InitializeCommands()
        {
            ViolationSummaryCommand = new DelegateCommand(() =>
            {
                PrepareViolationsSummaryAndChartData();

            });

            LoadCodeCommand = new DelegateCommand(() =>
            {                
                SelectedSourceCodeFile = default(DocumentData);
                SourceCodeFilesCollection?.Clear();
                ViolationSummaryCollection?.Clear();
                InitializePieChartProperties();
                //OrigCodeString = string.Empty;

                if (string.IsNullOrEmpty(CodeRootLocation))
                {
                    return;
                }
               
                var documentDataList = _folderScanner.GetFileListWithFullPath(CodeRootLocation).Where((x) => x != null).ToList<DocumentData>();

                SourceCodeFilesCollection.Clear();
                SourceCodeFilesCollection.AddRange(documentDataList);
            });

            AnalyzeCommand = new DelegateCommand(() =>
            {
                var selectedFilesForAnalysis = SourceCodeFilesCollection.Where((x) => x.IsChecked == true).ToList();
                //OutputText = _analysisManager.RunAnalysis(InputText);

                foreach (var codeFile in selectedFilesForAnalysis)
                {
                    string codeString = _codeParser.GetCodeFromFile(codeFile.FilePath);
                    //codeString =codeString.Replace("\r", "");

                    //codeString = InputText;
                    var analysisResult = _analysisManager.RunAnalysis(codeString);
                    codeFile.AnalysisResultDataInstance = analysisResult;
                    var status = (analysisResult.NameRuleErrors.Count == 0) ? AnalysisStatus.Completed_Ok : AnalysisStatus.Completed_NG;
                    codeFile.AnalysisStatusInstance = status;
                    //codeFile.OrigString = codeString;
                }

                PrepareViolationsSummaryAndChartData();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        private void PrepareViolationsSummaryAndChartData()
        {
            if (SourceCodeFilesCollection == null || SourceCodeFilesCollection?.Count == 0) { return; }

            ViolationSummaryCollection?.Clear();

            var unanalyzedFilesCount = SourceCodeFilesCollection.Count((x) => x.IsChecked == false);
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Unanalyzed File Count", SummaryCount = unanalyzedFilesCount });

            var analyzedFilesCount = SourceCodeFilesCollection.Count((x) => x.IsChecked == true);
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Analyzed File Count", SummaryCount = analyzedFilesCount });

            var okCodeFilesCount = SourceCodeFilesCollection.Count((x) => x.IsChecked == true && x.AnalysisStatusInstance == AnalysisStatus.Completed_Ok);
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "OK Code File Count", SummaryCount = okCodeFilesCount });

            var ngCodeFiles = SourceCodeFilesCollection.Where((x) => x.IsChecked == true && x.AnalysisStatusInstance == AnalysisStatus.Completed_NG).ToList();

            var ngCodeFilesCount = ngCodeFiles.Count;
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "NG Code File Count", SummaryCount = ngCodeFilesCount });
            var totalNGDetected = ngCodeFiles.Where((y) => y.AnalysisResultDataInstance?.NameRuleErrors.Count > 0).ToList();
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Total Violations Count", SummaryCount = totalNGDetected.Count });
            TotalViolationsCount = totalNGDetected.Count;

            var classNameViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
            Any((y) => y.Violation == NameRuleViolations.ClassNameRuleViolation));
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Class Violations", SummaryCount = classNameViolationsCount });
            ClassNameViolationsChartValue = new ChartValues<int> { classNameViolationsCount };


            var methodNameViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
           Any((y) => y.Violation == NameRuleViolations.MethodNameRuleViolation));
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Method Violations", SummaryCount = methodNameViolationsCount });
            MethodNameViolationsChartValue = new ChartValues<int> { methodNameViolationsCount };

            var namespaceNameViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
           Any((y) => y.Violation == NameRuleViolations.NamespaceRuleViolation));
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Namespace Violations", SummaryCount = namespaceNameViolationsCount });
            NamespaceNameViolationsChartValue = new ChartValues<int> { namespaceNameViolationsCount };


            var parameterNameViolations = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
          Any((y) => y.Violation == NameRuleViolations.ParameterNameRuleViolation));
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Parameter Violations", SummaryCount = parameterNameViolations });
            ParameterNameViolationsChartValue = new ChartValues<int> { parameterNameViolations };

            var privFieldViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
          Any((y) => y.Violation == NameRuleViolations.PrivateFieldNameRuleViolation));

            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Private Field Violations", SummaryCount = privFieldViolationsCount });
            PrivateFieldNameViolationsChartValue = new ChartValues<int> { privFieldViolationsCount };

            var protFieldViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
          Any((y) => y.Violation == NameRuleViolations.ProtectedFieldNameRuleViolation));
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Protected Field Violations", SummaryCount = protFieldViolationsCount });
            ProtectedFieldNameViolationsChartValue = new ChartValues<int> { protFieldViolationsCount };

            var pubFieldViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
          Any((y) => y.Violation == NameRuleViolations.PublicFieldNameRuleViolation));
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Public Field Violations", SummaryCount = pubFieldViolationsCount });
            PublicFieldNameViolationsChartValue = new ChartValues<int> { pubFieldViolationsCount };

            var privPropViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
        Any((y) => y.Violation == NameRuleViolations.PrivatePropertyNameRuleViolation));
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Private Property Violations", SummaryCount = privPropViolationsCount });
            PrivatePropertyNameViolationsChartValue = new ChartValues<int> { privPropViolationsCount };

            var protPropViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
    Any((y) => y.Violation == NameRuleViolations.ProtectedPropertyNameRuleViolation));
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Protected Property Violations", SummaryCount = protPropViolationsCount });
            ProtectedPropertyNameViolationsChartValue = new ChartValues<int> { protPropViolationsCount };

            var pubPropViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
    Any((y) => y.Violation == NameRuleViolations.PublicPropertyNameRuleViolation));
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Public Property Violations", SummaryCount = pubPropViolationsCount });
            PublicPropertyNameViolationsChartValue = new ChartValues<int> { pubPropViolationsCount };

            var largeMethodBodyViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
    Any((y) => y.Violation == NameRuleViolations.LargeMethodBodyRuleViolation));
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Large Method Body Violations", SummaryCount = largeMethodBodyViolationsCount });
            LargeMethodViolationsChartValue = new ChartValues<int> { largeMethodBodyViolationsCount };

        }
    }
}
