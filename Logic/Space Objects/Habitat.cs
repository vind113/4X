using Logic.Resourse;
using System;

namespace Logic.SpaceObjects {
    public abstract class SpaceBuilding : IHabitable {
        private double population;
        private double maximumPopulation;

        public virtual double Population {
            get => population;
            set {
                population = value;
            }
        }

        public virtual double MaximumPopulation {
            get => population;
            protected set {
                maximumPopulation = value;
            }
        }
    }

    /// <summary>
    /// Представляет обитаемую космическую станцию
    /// </summary>
    public class Habitat : SpaceBuilding {
        private readonly static short buildingTime = 24;
        private readonly static byte quality = 100;

        public Habitat() {
            this.MaximumPopulation = 20_000_000_000;
            this.Population = 0;
        }

        public static short BuildingTime { get => buildingTime; }
        public static byte Quality { get => quality; }
    }

    public class HabitatBuilder {
        private int buildingProgress;
        private readonly int buildingTarget;

        private readonly Resourses costPerTurn;

        public event EventHandler Completed;

        public HabitatBuilder() {
            this.costPerTurn = new Resourses(10E9, 100E9, 1E9);

            this.buildingProgress = 0;
            this.buildingTarget = Habitat.BuildingTime;
        }

        public void AddOneTurnProgressUsing(Resourses resourses) {
            if (resourses.CanSubstract(this.CostPerTurn)) {
                resourses.Substract(this.CostPerTurn);
                this.buildingProgress++;
            }

            if (this.buildingProgress == this.buildingTarget) {
                this.OnCompleted();
            }
        }

        public Resourses CostPerTurn { get => this.costPerTurn; }

        private void OnCompleted() {
            var handler = this.Completed;
            handler?.Invoke(this, new EventArgs());
        }
    }

    public class SystemBuildings {
        
    }
}
