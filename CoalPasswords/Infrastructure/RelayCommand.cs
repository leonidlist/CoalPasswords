using System;
using System.Windows.Input;

namespace CoalPasswords
{
    class RelayCommand : ICommand
    {
        readonly Action<object> action;
        readonly Predicate<object> predicate;
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
        public RelayCommand(Action<object> action, Predicate<object> predicate=null)
        {
            this.action = action;
            this.predicate = predicate;
        }
        public bool CanExecute(object parameter)
        {
            return predicate == null ? true : predicate.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            action?.Invoke(parameter);
        }
    }
}
