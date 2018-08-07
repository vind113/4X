using Logic.Resourse;
using System;
using System.Windows;

namespace Logic.PlayerClasses {
    public class Goods {
        private readonly static double hydrogenPerPerson = 0.005;
        private readonly static double commonMetalsPerPerson = 0.05;
        private readonly static double rareMetalsPerPerson = 0.00_001;

        private static Resourses previousTurnUsedResourses;

        public static Resourses PreviousTurnUsedResourses { get => previousTurnUsedResourses; }

        public static void SustainPopulationNeeds(Player player) {
            if (player == null) {
                throw new ArgumentNullException(nameof(player));
            }

            double playerPopulation = player.TotalPopulation;

            double hydrogen = hydrogenPerPerson * playerPopulation;
            double commonMetals = commonMetalsPerPerson * playerPopulation;
            double rareMetals = rareMetalsPerPerson * playerPopulation;

            Resourses neededResourses = new Resourses(hydrogen, commonMetals, rareMetals);

            previousTurnUsedResourses = neededResourses;

            try {
                player.OwnedResourses.Substract(neededResourses);
            }
            catch (ArgumentException) {
                player.OwnedResourses.SetToZero();
            }
        }
    }
}
