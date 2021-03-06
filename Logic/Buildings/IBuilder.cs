﻿using System;
using Logic.Resource;

namespace Logic.Buildings {
    public interface IBuilder {
        int BuildingProgress { get; }
        int BuildingDuration { get; }

        IComparableResources CostPerTurn { get; }

        event EventHandler<BuildingCompletedEventArgs> Completed;

        void OneTurnProgress(IMutableResources resources);
    }
}