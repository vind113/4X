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

        public Population Population { get => this.population; }
    }
}
