using Logic.Resource;
using System;
using System.Windows;

namespace Logic.PlayerClasses {
    public class CivilProduction {
        public readonly static double HYDROGEN_PER_PERSON = 0.005;
        public readonly static double COMMON_METALS_PER_PERSON = 0.05;
        public readonly static double RARE_METALS_PER_PERSON = 0.00001;

        public static Resources PreviousTurnUsedResources { get; private set; } = new Resources(0, 0, 0);

        public static void SustainPopulationNeeds(IPlayer player)
        {
            if (player == null) {
                throw new ArgumentNullException(nameof(player));
            }

            if(player.TotalPopulation <= 0) {
                return;
            }

            Resources neededResources = CalculatePopulationNeeds(player.TotalPopulation);

            if (player.OwnedResources.CanSubtract(neededResources)) {
                player.OwnedResources.Subtract(neededResources);
                PreviousTurnUsedResources = neededResources;
            }
            else {
                PreviousTurnUsedResources = new Resources(player.OwnedResources);
                player.OwnedResources.SetToZero();
            }
        }

        private static Resources CalculatePopulationNeeds(double population) {
            double hydrogen = HYDROGEN_PER_PERSON * population;
            double commonMetals = COMMON_METALS_PER_PERSON * population;
            double rareMetals = RARE_METALS_PER_PERSON * population;

            return new Resources(hydrogen, commonMetals, rareMetals);
        }
    }
}
