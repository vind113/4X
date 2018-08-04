using Logic.SpaceObjects;
using Logic.SupportClasses;
using System;

namespace Logic.PlayerClasses {
    [Serializable]
    public class CitizenHub {
        double citizensInHub = 0;
        double maximumCount = 1E8;

        public double CitizensInHub {
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

        public void MigrateHabitatToHub(IHabitable habitat) {
            double partOfTravellers = 0.001d;

            double citizensToHubExpected = Math.Floor(habitat.Population * partOfTravellers);

            double citizensToHub =
                Math.Floor(citizensToHubExpected * HelperRandomFunctions.GetRandomDouble());

            bool canTravelFromPlanet = citizensToHub < habitat.Population;
            bool canTravelToHub =
                (this.CitizensInHub + citizensToHub) < this.MaximumCount;

            if (canTravelFromPlanet && canTravelToHub) {
                habitat.Population -= citizensToHub;
                this.CitizensInHub += citizensToHub;
            }
        }

        public void MigrateHubToHabitat(IHabitable habitat) {
            double citizensFromHub =
                Math.Floor(HelperRandomFunctions.GetRandomDouble() * this.CitizensInHub);

            bool canTravelToPlanet = (habitat.Population + citizensFromHub) < habitat.MaximumPopulation;
            bool canTravelFromHub = citizensFromHub < this.CitizensInHub;

            if (canTravelToPlanet && canTravelFromHub) {
                this.CitizensInHub -= citizensFromHub;
                habitat.Population += citizensFromHub;
            }
        }
    }
}
