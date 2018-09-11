using System;
using Logic.SpaceObjects;
using Logic.PopulationClasses;

namespace Logic.Buildings {
    [Serializable]
    public abstract class SpaceBuilding : IHabitable {
        private Population population;

        public SpaceBuilding(long population, long maxPopulation) {
            this.population = new Population(population, maxPopulation);
        }

        public virtual long PopulationValue { get => population.Value; }
        public virtual long MaximumPopulation { get => population.MaxValue; }
        public Population Population { get => this.population; }
    }
}
