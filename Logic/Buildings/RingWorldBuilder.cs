using System;
using Logic.Resource;

namespace Logic.Buildings {
    public class RingWorldBuilder : IBuilder {
        public const int BuildingDuration = 1200;

        public int BuildingProgress => throw new NotImplementedException();

        public IComparableResources CostPerTurn => throw new NotImplementedException();

        public event EventHandler<SpaceBuildingCompletedEventArgs> Completed;

        public void OneTurnProgress(IMutableResources resources) {
            throw new NotImplementedException();
        }

        private void OnCompleted() {
            var handler = this.Completed;
            handler?.Invoke(this, new SpaceBuildingCompletedEventArgs(new RingWorld("RingWorld")));
        }
    }
}
