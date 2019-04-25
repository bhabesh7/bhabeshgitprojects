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
                RaisePropertyChanged(nameof(SelectedSourceCodeFile));
            }
        }



        public ObservableCollection<DocumentData> SourceCodeFilesCollection { get; set; }


        public MainViewModel(IFolderScanner folderScanner, ICodeParser codeParser, IAnalysisManager analysisManager)
        {
            _folderScanner = folderScanner;
            _codeParser = codeParser;
            _analysisManager = analysisManager;
            SourceCodeFilesCollection = new ObservableCollection<DocumentData>();
            CodeRootLocation = @"b:\samplefile";
            LoadCodeCommand = new DelegateCommand(() =>
            {
                //OpenFileDialog

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
                    codeFile.OrigString = codeString;
                }
            });
        }

    }
}
