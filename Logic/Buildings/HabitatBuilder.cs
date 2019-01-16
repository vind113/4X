using Logic.Resource;
using System;

namespace Logic.Buildings {
    [Serializable]
    public class HabitatBuilder : IBuilder {
        public const int BuildingDuration = 24;

        private string habitatName;

        public IComparableResources CostPerTurn { get; } = new ReadOnlyResources(10E9, 100E9, 100E6);
        public int BuildingProgress { get; private set; }

        public event EventHandler<SpaceBuildingCompletedEventArgs> Completed;

        public HabitatBuilder(string habitatName) {
            this.habitatName = habitatName;

            this.BuildingProgress = 0;
        }

        public void OneTurnProgress(IMutableResources resources) {
            if (resources.CanSubtract(this.CostPerTurn) && this.BuildingProgress < BuildingDuration) {
                resources.Subtract(this.CostPerTurn);
                this.BuildingProgress++;
            }

            if (this.BuildingProgress == BuildingDuration) {
                this.OnCompleted();
            }
        }

        private void OnCompleted() {
            var handler = this.Completed;
            handler?.Invoke(this, new SpaceBuildingCompletedEventArgs(new Habitat(this.habitatName)));
        }
    }
}
