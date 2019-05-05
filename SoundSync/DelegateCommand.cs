using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SoundSync
{
    public class DelegateCommand<T> : ICommand where T:class
    {
        readonly Action<T> _Execute;
        readonly Func<T, bool> _CanExecute;

        public DelegateCommand(Action<T> Execute, Func<T, bool> CanExecute = null)
        {
            this._Execute = Execute;
            this._CanExecute = CanExecute;
        }

        public void FireCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            if(_CanExecute != null)
            {
                return _CanExecute(parameter as T);
            }
            else
            {
                return true;
            }
        }

        public void Execute(object parameter)
        {
            _Execute(parameter as T);
        }
    }
}
