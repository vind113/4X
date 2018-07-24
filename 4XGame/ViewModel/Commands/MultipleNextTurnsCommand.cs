using Logic.GameClasses;
using System;
using System.Diagnostics;
using System.Windows.Threading;
using System.ComponentModel;

namespace _4XGame.ViewModel.Commands {
    public class MultipleNextTurnsCommand : CommandBase {
        private int commandProgress;
        private int commandParts = 1;
        private TimeSpan lastExecutionTime;

        public int CommandProgress {
            get => this.commandProgress;
            private set {
                this.commandProgress = value;
                OnPropertyChanged();
            }
        }

        public int CommandParts {
            get => this.commandParts;
            set {
                this.commandParts = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan LastExecutionTime {
            get => this.lastExecutionTime;
            protected set {
                this.lastExecutionTime = value;
                OnPropertyChanged();
            }
        }

        public override bool CanExecute(object parameter) {
            return true;
        }

        public override void Execute(object parameter) {
            Stopwatch stopwatch = Stopwatch.StartNew();
            this.Body(parameter);
            this.LastExecutionTime = stopwatch.Elapsed;
        }

        private void Body(object parameter) {
            Tuple<object, object> tuple = (Tuple<object, object>)parameter;

            string turnsToMakeString = (string)tuple.Item1;
            Game game = (Game)tuple.Item2;

            uint turnsToMake = 0;

            if (!UInt32.TryParse(turnsToMakeString, out turnsToMake)) {
                return;
            }

            commandParts = (int)turnsToMake;

            for (int i = 0; i < turnsToMake; i++) {
                Dispatcher.CurrentDispatcher.Invoke(()=> { game.NextTurn(); }, DispatcherPriority.Background);
                commandProgress++;
            }

            commandProgress = 0;
            commandParts = 1;

            Dispatcher.CurrentDispatcher.Invoke(() => { Console.Beep(); }, DispatcherPriority.Background);
        }
    }
}
