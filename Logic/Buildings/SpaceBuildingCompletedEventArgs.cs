using System;

namespace Logic.Buildings {
    public class SpaceBuildingCompletedEventArgs : EventArgs {
        private SpaceBuilding building;

        public SpaceBuildingCompletedEventArgs(SpaceBuilding building) {
            this.building = building ?? throw new ArgumentNullException(nameof(building));
        }

        public SpaceBuilding Building { get => this.building; }
    }
}
