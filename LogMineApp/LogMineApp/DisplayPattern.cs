using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMineApp
{
    public class DisplayPattern:BindableBase
    {
        private int logIndex;

        public int LogIndex
        {
            get { return logIndex; }
            set { logIndex = value; RaisePropertyChanged("LogIndex"); }
        }


        private string pattern;

        public string Pattern
        {
            get { return pattern; }
            set { pattern = value; RaisePropertyChanged(Pattern); }
        }

        private string representativeLogLine;

        public string RepresentativeLogLine
        {
            get { return representativeLogLine; }
            set { representativeLogLine = value; RaisePropertyChanged(RepresentativeLogLine); }
        }


        private int representativeIndex;

        public int RepresentativeIndex
        {
            get { return representativeIndex; }
            set { representativeIndex = value; RaisePropertyChanged("RepresentativeIndex"); }
        }


        private string allIndexesInClusterString;

        public string AllIndexesinClusterString
        {
            get { return allIndexesInClusterString; }
            set { allIndexesInClusterString = value; RaisePropertyChanged(AllIndexesinClusterString); }
        }

        private int level;

        public int Level
        {
            get { return level; }
            set { level = value; RaisePropertyChanged("Level"); }
        }



    }
}
