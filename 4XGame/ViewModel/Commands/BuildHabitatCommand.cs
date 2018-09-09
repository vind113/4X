using Logic.Buildings;
using Logic.SpaceObjects;

namespace _4XGame.ViewModel.Commands {
    public class BuildHabitatCommand : CommandBase {
        public override bool CanExecute(object parameter) {
            return (parameter != null && parameter is StarSystem);
        }

        public override void Execute(object parameter) {
            if(parameter is StarSystem system) {
                system.Buildings.BuildNew(new HabitatBuilder(
                    $"{system.Name} Habitat #{system.Buildings.Existing.Count + system.Buildings.InConstruction.Count + 1}"
                ));
            }
        }
    }
}
