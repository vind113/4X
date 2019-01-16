using System;
using Logic.Resource;

namespace Logic.Buildings {
    public interface IBuilder {
        int BuildingProgress { get; }

        IComparableResources CostPerTurn { get; }

        event EventHandler<SpaceBuildingCompletedEventArgs> Completed;

        void OneTurnProgress(IMutableResources resources);
    }
}