using Logic.GameClasses;
using Logic.Space_Objects;
using System;

namespace _4XGame.ViewModel.Commands {
    public class SelectCelestialBodyCommand : CommandBase {
        public override bool CanExecute(object parameter) {
            return true;
        }

        public override void Execute(object parameter) {
            Tuple<object, object> tuple = (Tuple<object, object>)parameter;

            MainWindowViewModel viewModel = (MainWindowViewModel)tuple.Item1;
            
            if(tuple.Item2 == null) {
                return;
            }

            if(tuple.Item2 is Planet planet) {
                viewModel.SelectedPlanet = planet;
            }
            else if(tuple.Item2 is Star star) {
                viewModel.SelectedStar = star;
            }
        }
    }
}
