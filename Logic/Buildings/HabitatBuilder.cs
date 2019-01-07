﻿using Logic.Resource;
using System;

namespace Logic.Buildings {
    [Serializable]
    public class HabitatBuilder {
        private readonly int buildingTarget;
        private string habitatName;

        public Resources CostPerTurn { get; } = new Resources(10E9, 100E9, 100E6);
        public int BuildingProgress { get; private set; }

        public event EventHandler<SpaceBuildingCompletedEventArgs> Completed;

        public HabitatBuilder(string habitatName) {
            this.habitatName = habitatName;

            this.BuildingProgress = 0;
            this.buildingTarget = Habitat.BuildingTime;
        }

        public void OneTurnProgress(Resources resources) {
            if (resources.CanSubtract(this.CostPerTurn) && this.BuildingProgress < this.buildingTarget) {
                resources.Subtract(this.CostPerTurn);
                this.BuildingProgress++;
            }

            if (this.BuildingProgress == this.buildingTarget) {
                this.OnCompleted();
            }
        }

        private void OnCompleted() {
            var handler = this.Completed;
            handler?.Invoke(this, new SpaceBuildingCompletedEventArgs(new Habitat(this.habitatName)));
        }
    }
}