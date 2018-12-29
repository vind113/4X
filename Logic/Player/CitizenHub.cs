using Logic.PopulationClasses;
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

        public void MigrateToHub(Population habitatPopulation) {
            double partOfTravellers = 0.001d;

            double citizensToHubExpected = Math.Floor(habitatPopulation.Value * partOfTravellers);

            long citizensFromHabitat =
                (long)Math.Floor(citizensToHubExpected * HelperRandomFunctions.GetRandomDouble());

            bool canTravelFromPlanet = citizensFromHabitat < habitatPopulation.Value;
            bool canTravelToHub =
                (this.CitizensInHub + citizensFromHabitat) < this.MaximumCount;

            if (canTravelFromPlanet && canTravelToHub) {
                habitatPopulation.Substract(citizensFromHabitat);
                this.CitizensInHub += citizensFromHabitat;
            }
        }

        public void MigrateFromHub(Population habitatPopulation) {
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
