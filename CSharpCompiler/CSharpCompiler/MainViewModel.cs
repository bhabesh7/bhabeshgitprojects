using Accord.DataModel;
using Accord.Interfaces;
using CSharpCompilerLib;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSharpCompiler
{
    public class MainViewModel : BindableBase
    {
        private string textInput;

        public string InputText
        {
            get { return textInput; }
            set
            {
                textInput = value;
                RaisePropertyChanged(nameof(InputText));
            }
        }

        private string textOutput;

        public string OutputText
        {
            get { return textOutput; }
            set
            {
                textOutput = value;
                RaisePropertyChanged(nameof(OutputText));

            }
        }



            private ICommand analyzeCommand;

            public ICommand AnalyzeCommand
            {
                get { return analyzeCommand; }
                set { analyzeCommand = value; RaisePropertyChanged(nameof(AnalyzeCommand)); }
            }

        IFolderScanner _folderScanner;
        ICodeParser _codeParser;
        IAnalysisManager _analysisManager;

        public ObservableCollection<DocumentData> SourceCodeFilesCollection { get; set; }

        public MainViewModel()//(IFolderScanner folderScanner, ICodeParser codeParser, IAnalysisManager analysisManager)
        {

            //_folderScanner = MEFLoader.folderScanner;
            //_codeParser = MEFLoader.codeParser;
            //_analysisManager = MEFLoader.analysisManager;
            
            AnalyzeCommand = new DelegateCommand(() =>
             {                 
                 OutputText = _analysisManager.RunAnalysis(InputText);
             });
        }


    }
}
