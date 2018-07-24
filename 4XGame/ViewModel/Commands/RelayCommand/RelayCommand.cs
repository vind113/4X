﻿using System;
using System.Windows.Input;

namespace _4XGame.ViewModel.Commands {
    public class RelayCommand<T> : CommandBase {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        public RelayCommand(Action<T> execute) : this(execute, null) { }
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute) {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public override bool CanExecute(object parameter) {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public override void Execute(object parameter) {
            _execute((T)parameter);
        }
    }
}
