using _4XGame.Updaters;
using Logic.GameClasses;
using System.Diagnostics;

namespace _4XGame.ViewModel.Commands {
    public class NextTurnCommand : CommandBase {

        public override bool CanExecute(object parameter) {
            if(parameter != null && parameter is Game) {
                return true;
            }
            return false;
        }

        public override void Execute(object parameter) {
            Stopwatch stopwatch = Stopwatch.StartNew();

            if (parameter != null && parameter is Game game) {
                game.NextTurn();
                new PlayerInfoUpdater(game.Player).UpdatePopulation();
            }
        }
    }
}
