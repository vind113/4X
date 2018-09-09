using System;
using Logic.SpaceObjects;

namespace Logic.Buildings {
    [Serializable]
    public abstract class SpaceBuilding : IHabitable {
        private double population;
        private double maximumPopulation;

        public virtual double Population {
            get => population;
            set {
                if (value >= 0 && value <= this.MaximumPopulation) population = value;
            }
        }

        public virtual double MaximumPopulation {
            get => maximumPopulation;
            protected set {
                maximumPopulation = value;
            }
        }
    }
}
