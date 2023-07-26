using System;
using System.Windows.Input;

namespace Tamarillo.Classes.Commands {

    public class RelayGenericParamCommand<T> : ICommand {

        public Predicate<T> CanExecuteDelegate {
            get; set;
        }
        public Action<T> ExecuteDelegate {
            get; set;
        }

        public RelayGenericParamCommand(Action<T> action) {
            ExecuteDelegate = action;
        }

        #region ICommand Members

        public bool CanExecute(object parameter) {
            return CanExecuteDelegate == null || CanExecuteDelegate((T)parameter);
        }

        public event EventHandler CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public void Execute(object parameter) {
            ExecuteDelegate?.Invoke((T)parameter);
        }

        #endregion

    }

}