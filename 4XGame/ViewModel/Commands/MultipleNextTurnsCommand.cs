﻿using _4XGame.Updaters;
using Logic.GameClasses;
using System;
using System.Diagnostics;
using System.Windows.Threading;

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
            private set {
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

            if (parameter != null && parameter is Tuple<object, object> tuple) {
                if (tuple.Item1 is string turns && tuple.Item2 is Game) {
                    if (UInt32.TryParse(turns, out uint number)) {
                        return true;
                    }
                }
            }

            return false;
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

            if (!uint.TryParse(turnsToMakeString, out turnsToMake)) {
                return;
            }

            CommandParts = (int)turnsToMake;
            DispatcherPriority gameActionsPriority = DispatcherPriority.ContextIdle;

            for (int i = 0; i < turnsToMake; i++) {
                Dispatcher.CurrentDispatcher.Invoke(()=> { game.NextTurn(); }, gameActionsPriority);
                CommandProgress++;
            }

            Dispatcher.CurrentDispatcher.Invoke(() => {
                new PlayerInfoUpdater(game.Player).UpdatePopulation();
            }, gameActionsPriority);

            CommandProgress = 0;
            CommandParts = 1;

            Dispatcher.CurrentDispatcher.Invoke(() => { Console.Beep(); }, gameActionsPriority);
        }
    }
}
