﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageFastLoader.DataModel
{
    public class DocumentData : BaseNotify
    {
        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                RaisePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("FileName"));
            }
        }

        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                RaisePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("FilePath"));
            }
        }

        public DocumentData()
        {
            FilePath = string.Empty;
            FileName = string.Empty;
        }

    }
}
