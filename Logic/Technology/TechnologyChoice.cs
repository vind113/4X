using System;

namespace Logic.TechnologyClasses {
    public enum Technologies {
        Empty,
        PopulationGrowth
    }

    public class TechnologyChoice {
        private readonly Technology technology;

        public TechnologyChoice(Technologies technology) {
            this.technology = this.GetTechnology(technology);
        }

        private Technology GetTechnology(Technologies choice) {
            switch (choice) {
                case Technologies.Empty:
                    return null;
                case Technologies.PopulationGrowth:
                    return new PopulationGrowthTechnology(1);
                default:
                    throw new ArgumentException("Incorrect argument: no such technology");
            }
        }
    }
}