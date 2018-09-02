using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneSearch.DataModel
{
    public class SearchFilterData : BaseNotify
    {

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("Name"));

            }
        }

        private string _filter;

        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                RaisePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("Filter"));
            }
        }


        private bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
                RaisePropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs("IsChecked"));

            }
        }

        public SearchFilterData(string name, string filter, bool isChecked)
        {
            Name = name;
            Filter = filter;
            IsChecked = isChecked;
        }
    }
}
