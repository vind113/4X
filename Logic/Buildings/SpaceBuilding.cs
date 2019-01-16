using System;
using Logic.SpaceObjects;
using Logic.PopulationClasses;

namespace Logic.Buildings {
    [Serializable]
    public abstract class SpaceBuilding : IHabitable {
        private readonly Population population;

        public string Name { get; private set; }
        public Population Population { get; }

        public SpaceBuilding(string name, long population, long maxPopulation) {
            this.Name = name;
            this.population = new Population(population, maxPopulation);
        }

        public void Colonize() {
            this.Population.Add(10_000_000);
        }
    }
}
