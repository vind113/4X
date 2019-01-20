using System;

namespace Logic.Buildings {
    public class BuildingCompletedEventArgs : EventArgs {
        public SpaceBuilding Building { get; }

        public BuildingCompletedEventArgs(SpaceBuilding building) {
            this.Building = building ?? throw new ArgumentNullException(nameof(building));
        }
    }
}
