using Logic.Resourse;
using System;

namespace Logic.Buildings {
    /// <summary>
    /// Представляет обитаемую космическую станцию
    /// </summary>
    public class Habitat : SpaceBuilding {
        private readonly static short buildingTime = 24;
        private readonly static byte quality = 100;

        private string name;

        public Habitat(string name) {
            this.name = name;

            this.MaximumPopulation = 20_000_000_000;
            this.Population = 0;
        }

        public static short BuildingTime { get => buildingTime; }
        public static byte Quality { get => quality; }
        public string Name { get => this.name; }
    }

    [Serializable]
    public class HabitatBuilder {
        private int buildingProgress;
        private readonly int buildingTarget;

        private readonly Resourses costPerTurn;

        private string habitatName;

        public event EventHandler<SpaceBuildingCompletedEventArgs> Completed;

        public HabitatBuilder(string habitatName) {
            this.habitatName = habitatName;

            this.costPerTurn = new Resourses(10E9, 100E9, 1E8);

            this.buildingProgress = 0;
            this.buildingTarget = Habitat.BuildingTime;
        }

        public void OneTurnProgress(Resourses resourses) { 
            if (resourses.CanSubstract(this.CostPerTurn) && this.buildingProgress < this.buildingTarget) {
                resourses.Substract(this.CostPerTurn);
                this.buildingProgress++;
            }
            
            if (this.buildingProgress == this.buildingTarget) {
                this.OnCompleted();
            }
        }

        public Resourses CostPerTurn { get => this.costPerTurn; }
        public int BuildingProgress { get => this.buildingProgress; }

        private void OnCompleted() {
            var handler = this.Completed;
            handler?.Invoke(this, new SpaceBuildingCompletedEventArgs(new Habitat(this.habitatName)));
        }
    }
}
