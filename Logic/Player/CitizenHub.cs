using Logic.PopulationClasses;
using Logic.SupportClasses;
using System;

namespace Logic.PlayerClasses {
    [Serializable]
    public class CitizenHub {
        private const double partOfTravellers = 0.001d;
        long citizensInHub = 0;
        double maximumCount = 1E8;

        public long CitizensInHub {
            get => citizensInHub;
            private set {
                if (value <= maximumCount && value >= 0) {
                    citizensInHub = value;
                }
            }
        }

        public double MaximumCount {
            get => maximumCount;
            private set {
                if (value >= 0) {
                    maximumCount = value;
                }
            }
        }

        public void SetCitizenHubCapacity(long population) {
            long newHubCapacity = population / 1000;

            if (newHubCapacity > this.CitizensInHub) {
                this.MaximumCount = newHubCapacity;
            }
        }

        public void ConductMigration(Population population) {
            this.MigrateToHub(population);
            this.MigrateFromHub(population);
        }

        private void MigrateToHub(Population habitatPopulation) {
            long citizensToHubExpected = (long)Math.Floor(habitatPopulation.Value * partOfTravellers);

            long citizensFromHabitat =
                (long)Math.Floor(citizensToHubExpected * HelperRandomFunctions.GetRandomDouble());

            bool canTravelFromPlanet = citizensFromHabitat < habitatPopulation.Value;
            bool canTravelToHub =
                (this.CitizensInHub + citizensFromHabitat) < this.MaximumCount;

            if (canTravelFromPlanet && canTravelToHub) {
                habitatPopulation.Subtract(citizensFromHabitat);
                this.CitizensInHub += citizensFromHabitat;
            }
        }

        private void MigrateFromHub(Population habitatPopulation) {
            long citizensToHabitat =
                (long)Math.Floor(HelperRandomFunctions.GetRandomDouble() * this.CitizensInHub);

            bool canTravelToPlanet = (habitatPopulation.Value + citizensToHabitat) < habitatPopulation.MaxValue;
            bool canTravelFromHub = citizensToHabitat < this.CitizensInHub;

            if (canTravelToPlanet && canTravelFromHub) {
                this.CitizensInHub -= citizensToHabitat;
                habitatPopulation.Add(citizensToHabitat);
            }
        }
    }
}
