using Logic.GameClasses;
using System;
using System.Diagnostics;
using System.Windows.Threading;

namespace _4XGame.ViewModel.Commands {
    public class MultipleNextTurnsCommand : CommandBase {
        public override bool CanExecute(object parameter) {
            return true;
        }

        public override void Execute(object parameter) {
            Stopwatch stopwatch = Stopwatch.StartNew();
            this.Body(parameter);
            this.LastExecutionTime = stopwatch.Elapsed;
        }

        private void Body(object parameter) {
            Tuple<string, object> tuple = (Tuple<string, object>)parameter;
            Game game = (Game)tuple.Item2;

            int turnsToMake = 0;
            if (!Int32.TryParse(tuple.Item1, out turnsToMake)) {
                return;
            }

            for (int i = 0; i < turnsToMake; i++) {
                Dispatcher.CurrentDispatcher.Invoke(()=> { game.NextTurn(); }, DispatcherPriority.Background);
            }
        }
    }
}
