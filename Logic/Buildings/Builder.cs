using System;
using Logic.Resource;

namespace Logic.Buildings {
    [Serializable]
    public abstract class Builder : IBuilder {
        public int BuildingProgress { get; protected set; } = 0;
        public int BuildingDuration { get; }
        public IComparableResources CostPerTurn { get; }
        public SpaceBuilding BuildingBeingBuilt { get; }

        public event EventHandler<BuildingCompletedEventArgs> Completed;

        protected Builder(int duration, IBasicResources costPerTurn, SpaceBuilding building) {
            this.BuildingDuration = duration;
            this.CostPerTurn = new ReadOnlyResources(costPerTurn);
            this.BuildingBeingBuilt = building;
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
            handler?.Invoke(this, new BuildingCompletedEventArgs(this.BuildingBeingBuilt));
        }
    }
}
