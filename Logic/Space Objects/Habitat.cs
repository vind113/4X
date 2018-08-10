using Logic.Resourse;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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

        public event EventHandler<SpaceBuildingCompletedEventArgs> Completed;

        public HabitatBuilder() {
            this.costPerTurn = new Resourses(10E9, 100E9, 1E9);

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
            handler?.Invoke(this, new SpaceBuildingCompletedEventArgs(new Habitat()));
        }
    }

    public class SystemBuildings {
        private List<SpaceBuilding> existing;
        private List<HabitatBuilder> inConstruction;

        public SystemBuildings() {
            this.existing = new List<SpaceBuilding>();
            this.inConstruction = new List<HabitatBuilder>();
        }

        public void BuildNew(HabitatBuilder builder) {
            if (builder == null) {
                throw new ArgumentNullException(nameof(builder));
            }

            if (this.inConstruction.Contains(builder)) {
                return;
            }
            
            this.inConstruction.Add(builder);
            builder.Completed += AddCompleted;
        }

        public void NextTurn(Resourses resourses) {
            foreach (var building in inConstruction.ToArray()) {
                building.OneTurnProgress(resourses);
            }
        }

        #region Properties
        public int ExistingCount { get => this.existing.Count; }
        public int InConstructionCount { get => this.inConstruction.Count; }

        public ReadOnlyCollection<SpaceBuilding> Existing {
            get => new ReadOnlyCollection<SpaceBuilding>(this.existing);
        }

        public ReadOnlyCollection<HabitatBuilder> InConstruction {
            get => new ReadOnlyCollection<HabitatBuilder>(this.inConstruction);
        }
        #endregion

        private void AddCompleted(object sender, SpaceBuildingCompletedEventArgs e) {
            if (sender is HabitatBuilder builder) {
                this.inConstruction.Remove(builder);
                this.existing.Add(e.Habitat);
            }
        }
    }
}
