using Logic.PlayerClasses;
using Logic.SpaceObjects;
using System;

namespace _4XGame.ViewModel.Commands {
    public class ColonizePlanetCommand : CommandBase {
        public override bool CanExecute(object parameter) {
            return true;
        }

        public override void Execute(object parameter) {
            Tuple<object, object> tuple = (Tuple<object, object>)parameter;

            Player player = (Player)tuple.Item1;
            Planet planet = (Planet)tuple.Item2;

            if (tuple.Item1 != null && tuple.Item2 != null) {
                planet.Colonize(player);
            }
        }
    }
}
