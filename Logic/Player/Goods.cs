﻿using Logic.Resource;
using System;
using System.Windows;

namespace Logic.PlayerClasses {
    public class CivilProduction {
        public readonly static double HYDROGEN_PER_PERSON = 0.005;
        public readonly static double COMMON_METALS_PER_PERSON = 0.05;
        public readonly static double RARE_METALS_PER_PERSON = 0.00001;

        public static Resources PreviousTurnUsedResources { get; private set; } = new Resources(0, 0, 0);

        public static void SustainPopulationNeeds(long population, Resources from)
        {
            if (from == null) {
                throw new ArgumentNullException(nameof(from));
            }

            if (population <= 0) {
                return;
            }

            Resources neededResources = CalculatePopulationNeeds(population);

            if (from.CanSubtract(neededResources)) {
                from.Subtract(neededResources);
                PreviousTurnUsedResources = neededResources;
            }
            else {
                PreviousTurnUsedResources = new Resources(from);
                from.SetToZero();
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
