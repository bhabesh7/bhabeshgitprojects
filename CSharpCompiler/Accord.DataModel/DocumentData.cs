using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accord.DataModel
{
    public class DocumentData
    {
        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                //RaisePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("FileName"));
            }
        }

        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                //RaisePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("FilePath"));
            }
        }

        private string _extention;

        public string Extention
        {
            get { return _extention; }
            set
            {
                _extention = value;
                //RaisePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("Extention"));
            }
        }

        private bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; }
        }

        private AnalysisResultData _analysisResultData;

        public AnalysisResultData AnalysisResultDataInstance
        {
            get { return _analysisResultData; }
            set { _analysisResultData = value; }
        }

        private string _origString;

        public string OrigString
        {
            get { return _origString; }
            set { _origString = value; }
        }


        public DocumentData()
        {
            FilePath = string.Empty;
            FileName = string.Empty;
            IsChecked = true;
        }

    }
}
