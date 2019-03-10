using LogMineLib.Impl;
using LogMineLib.Interfaces;
using LogMineLib.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LogMineApp
{
    public class MainWindowViewModel : BindableBase
    {
        public ICommand AnalyzeCommand { get; set; }
        public ObservableCollection<DisplayPattern> PatternsCollection { get; set; }
        public ObservableCollection<DisplayPattern> RawLogCollection { get; set; }

        private string clusterSummary;

        public string ClusterSummary
        {
            get { return clusterSummary; }
            set { clusterSummary = value; RaisePropertyChanged("ClusterSummary"); }
        }


        public MainWindowViewModel()
        {
            RawLogCollection = new ObservableCollection<DisplayPattern>();
            PatternsCollection = new ObservableCollection<DisplayPattern>();
            AnalyzeCommand = new DelegateCommand(AnalyzeCommandHandler);
        }

        private async void AnalyzeCommandHandler()
        {
            ILogTree logTree = new BasicLogTree();
            var clusters = logTree.BuildLogTree();

            int logIndexTemp = 0;
            foreach (var item in logTree.GetAllLogLines().Select((x) => new DisplayPattern { Pattern = x, LogIndex= logIndexTemp++}))
            {
                RawLogCollection.Add(item);
            }

            foreach (var item in clusters)
            {
                var clusterObject = new DisplayPattern
                {
                    Pattern = item.PatternLine,
                    Level = item.Level,
                    RepresentativeIndex = item.ClusterInfo.RepresentativeIndex,
                    RepresentativeLogLine = item.Line.Line
                };
                clusterObject.AllIndexesinClusterString = item.ClusterInfo.AllIndexesInCluster.Select(t=> t.ToString()).
                    Aggregate((x, y) => x + "," + y);
                PatternsCollection.Add(clusterObject);
            }

            ClusterSummary = string.Format("{0} unique cluster patterns found", clusters.Count);
            //LineNode tempNode = lineNode;

            //while (tempNode != null)
            //{
            //    PatternsCollection.Add(new DisplayPattern { Pattern = tempNode.PatternLine });
            //    tempNode = tempNode.RightSiblingNode;
            //}
        }

        //private async Task<LineNode> BuildLogTreeAsync()
        //{
        //    ILogTree logTree = new BasicLogTree();
        //    await logTree.BuildLogTree();
        //}

    }
}
