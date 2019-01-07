using Logic.PlayerClasses;
using Logic.SpaceObjects;
using System;

namespace _4XGame.ViewModel.Commands {
    public class ColonizePlanetCommand : CommandBase {
        public override bool CanExecute(object parameter) {
            Tuple<object, object> tuple = parameter as Tuple<object, object>;

            if (tuple == null || tuple.Item1 == null || tuple.Item2 == null) {
                return false;
            }

            Player player = tuple.Item1 as Player;
            Planet planet = tuple.Item2 as Planet;

            if (player != null && planet != null) {
                if (planet.Population.MaxValue > 0 && planet.Population.Value == 0) {
                    return true;
                }
            }

            return false;
        }

        public override void Execute(object parameter) {
            Tuple<object, object> tuple = (Tuple<object, object>)parameter;

            if (tuple.Item1 is Player player && tuple.Item2 is Planet planet) {
                planet.Colonize(player);
            }
        }
    }
}
