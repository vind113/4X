using Logic.SpaceObjects;
using Logic.SupportClasses;
using System;

namespace Logic.PlayerClasses {
    [Serializable]
    public class CitizenHub {
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
            set {
                if (value >= 0) {
                    maximumCount = value;
                }
            }
        }

        public void MigrateToHub(IHabitable habitat) {
            double partOfTravellers = 0.001d;

            double citizensToHubExpected = Math.Floor(habitat.PopulationValue * partOfTravellers);

            long citizensFromHabitat =
                (long)Math.Floor(citizensToHubExpected * HelperRandomFunctions.GetRandomDouble());

            bool canTravelFromPlanet = citizensFromHabitat < habitat.PopulationValue;
            bool canTravelToHub =
                (this.CitizensInHub + citizensFromHabitat) < this.MaximumCount;

            if (canTravelFromPlanet && canTravelToHub) {
                habitat.Population.Substract(citizensFromHabitat);
                this.CitizensInHub += citizensFromHabitat;
            }
        }

        public void MigrateFromHub(IHabitable habitat) {
            long citizensToHabitat =
                (long)Math.Floor(HelperRandomFunctions.GetRandomDouble() * this.CitizensInHub);

            bool canTravelToPlanet = (habitat.PopulationValue + citizensToHabitat) < habitat.MaximumPopulation;
            bool canTravelFromHub = citizensToHabitat < this.CitizensInHub;

            if (canTravelToPlanet && canTravelFromHub) {
                this.CitizensInHub -= citizensToHabitat;
                habitat.Population.Add(citizensToHabitat);
            }
        }
    }
}
