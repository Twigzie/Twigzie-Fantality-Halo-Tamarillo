using System;
using System.Windows.Input;

namespace Tamarillo.Classes.Commands {

    public class RelayParamCommand : ICommand {

        private readonly Action<object> _baseAction;
        private readonly Func<bool> _basePredicate;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public RelayParamCommand(Action<object> action) : this(action, null) {
            _baseAction = action;
        }
        public RelayParamCommand(Action<object> action, Func<bool> predicate) {
            _baseAction = action;
            _basePredicate = predicate;
        }

        public void Execute(object parameter) {
            _baseAction(parameter);
        }
        public bool CanExecute(object parameter) {
            return _basePredicate == null || _basePredicate();
        }

    }

}