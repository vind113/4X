using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace _4XGame.ViewModel.Commands {

    public abstract class CommandBase : ICommand, INotifyPropertyChanged {
        private TimeSpan lastExecutionTime;

        public event PropertyChangedEventHandler PropertyChanged;

        public TimeSpan LastExecutionTime {
            get => this.lastExecutionTime;
            protected set {
                this.lastExecutionTime = value;
                OnPropertyChanged();
            }
        }

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);

        public event EventHandler CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}