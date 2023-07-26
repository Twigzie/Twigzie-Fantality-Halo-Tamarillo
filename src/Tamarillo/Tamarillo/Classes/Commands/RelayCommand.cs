using System;
using System.Windows.Input;

namespace Tamarillo.Classes.Commands {

    public class RelayCommand : ICommand {

        private readonly Action _baseAction;
        private readonly Func<bool> _basePredicate;

        public event EventHandler CanExecuteChanged = (sender, e) => { };

        public RelayCommand(Action action, Func<bool> predicate = null) {
            _baseAction = action ?? throw new ArgumentNullException(nameof(action));
            _basePredicate = predicate;
        }

        public void Execute(object parameter) {
            _baseAction();
        }
        public bool CanExecute(object parameter) {
            return _basePredicate == null || _basePredicate();
        }

    }

}