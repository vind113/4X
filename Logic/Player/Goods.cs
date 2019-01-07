﻿using Logic.Resourse;
using System;
using System.Windows;

namespace Logic.PlayerClasses {
    public class CivilProduction {
        public readonly static double HYDROGEN_PER_PERSON = 0.005;
        public readonly static double COMMON_METALS_PER_PERSON = 0.05;
        public readonly static double RARE_METALS_PER_PERSON = 0.00001;

        public static Resourses PreviousTurnUsedResourses { get; private set; } = new Resourses(0, 0, 0);

        public static void SustainPopulationNeeds(IPlayer player)
        {
            if (player == null) {
                throw new ArgumentNullException(nameof(player));
            }

            if(player.TotalPopulation <= 0) {
                return;
            }

            Resourses neededResourses = CalculatePopulationNeeds(player.TotalPopulation);

            if (player.OwnedResourses.CanSubtract(neededResourses)) {
                player.OwnedResourses.Substract(neededResourses);
                PreviousTurnUsedResourses = neededResourses;
            }
            else {
                PreviousTurnUsedResourses = new Resourses(player.OwnedResourses);
                player.OwnedResourses.SetToZero();
            }
        }

        private static Resourses CalculatePopulationNeeds(double population) {
            double hydrogen = HYDROGEN_PER_PERSON * population;
            double commonMetals = COMMON_METALS_PER_PERSON * population;
            double rareMetals = RARE_METALS_PER_PERSON * population;

            return new Resourses(hydrogen, commonMetals, rareMetals);
        }
    }
}
