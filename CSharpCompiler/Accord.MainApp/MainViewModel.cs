using Accord.DataModel;
using Accord.Interfaces;
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

        public MainViewModel(IFolderScanner folderScanner, ICodeParser codeParser, IAnalysisManager analysisManager)
        {
            _folderScanner = folderScanner;
            _codeParser = codeParser;
            _analysisManager = analysisManager;
            SourceCodeFilesCollection = new ObservableCollection<DocumentData>();
            ViolationSummaryCollection = new ObservableCollection<SummaryData>();
            CodeRootLocation = @"b:\samplefile";
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            ViolationSummaryCommand = new DelegateCommand(() =>
            {
                PrepareViolationsSummary();

            });

            LoadCodeCommand = new DelegateCommand(() =>
            {
                //OpenFileDialog

                //OrigCodeString = string.Empty;
                SelectedSourceCodeFile = null;
                SourceCodeFilesCollection?.Clear();
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

                PrepareViolationsSummary();
            });
        }

        private void PrepareViolationsSummary()
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
            //ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Total Violation Count", SummaryCount = totalNGDetected.Count });

            var classViolations = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
            Any((y) => y.Violation == NameRuleViolations.ClassNameRuleViolation));
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Class Violations", SummaryCount = classViolations });

            var methViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
           Any((y) => y.Violation == NameRuleViolations.MethodNameRuleViolation));

            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Method Violations", SummaryCount = methViolationsCount });

            var nsViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
          Any((y) => y.Violation == NameRuleViolations.NamespaceRuleViolation));
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Namespace Violations", SummaryCount = nsViolationsCount });


            var paramViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
          Any((y) => y.Violation == NameRuleViolations.ParameterNameRuleViolation));

            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Parameter Violations", SummaryCount = paramViolationsCount });


            var privFieldViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
          Any((y) => y.Violation == NameRuleViolations.PrivateFieldNameRuleViolation));

            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Private Field Violations", SummaryCount = privFieldViolationsCount });


            var protFieldViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
          Any((y) => y.Violation == NameRuleViolations.ProtectedFieldNameRuleViolation));

            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Protected Field Violations", SummaryCount = protFieldViolationsCount });


            var pubFieldViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
          Any((y) => y.Violation == NameRuleViolations.PublicFieldNameRuleViolation));

            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Public Field Violations", SummaryCount = pubFieldViolationsCount });


            var privPropViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
        Any((y) => y.Violation == NameRuleViolations.PrivatePropertyNameRuleViolation));

            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Private Property Violations", SummaryCount = privPropViolationsCount });


            var protPropViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
    Any((y) => y.Violation == NameRuleViolations.ProtectedPropertyNameRuleViolation));
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Protected Property Violations", SummaryCount = protPropViolationsCount });

            var pubPropViolationsCount = totalNGDetected.Count((x) => x.AnalysisResultDataInstance.NameRuleErrors.
    Any((y) => y.Violation == NameRuleViolations.PublicPropertyNameRuleViolation));
            ViolationSummaryCollection.Add(new SummaryData { SummaryName = "Public Property Violations", SummaryCount = pubPropViolationsCount });
        }
    }
}
