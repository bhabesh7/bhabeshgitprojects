using Accord.DataModel;
using Accord.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Accord.ViewModule.Properties
{
    public class AccordViewModel: BindableBase
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

    }
}
