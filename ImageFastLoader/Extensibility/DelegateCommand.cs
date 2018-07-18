using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LuceneSearch.Extensibility
{
    public class DelegateCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if(_canExecute ==null)
            {
                return false;
            }

            _canExecute.Invoke(parameter);
            return true;
        }

        public void Execute(object parameter)
        {
            if (_execute == null)
            {
                //
                return;
            }
            _execute.Invoke(parameter);
        }


        Func<object, bool> _canExecute;
        Action<object> _execute;
        public DelegateCommand(Func<object, bool> canExecute, Action<object> execute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }
    }
}
