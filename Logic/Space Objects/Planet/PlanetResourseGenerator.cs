using System;
using Logic.Resource;
using Logic.SupportClasses;

namespace Logic.SpaceObjects.PlanetClasses {
    public class PlanetResourceGenerator {
        private const double MASS_OF_TEN_KM_CRUST = 10d * ((3d) * 10E9);

        private const int FRACTION_OF_HYDROGEN = 200;
        private const double GAS_GIANT_HYDROGEN_MULTIPLIER = 1.4 * 20 * 1E3 * 1E9;

        private const int FRACTION_OF_COMMON_METALS_TERRA = 20;
        private const int FRACTION_OF_COMMON_METALS_FERRIA = 2;

        private const double FRACTION_OF_RARE_METALS_TERRA = 1E5;
        private const double FRACTION_OF_RARE_METALS_FERRIA = 1E4;

        public PlanetResourceGenerator() {

        }

        public Resources GenerateFor(Planet planet) {
            switch (planet.Type.SubstancesClass) {
                case SubstancesClass.Ferria:
                    return GenerateForFerria(planet.Area);
                case SubstancesClass.Terra:
                    return GenerateForTerra(planet.Area);
                case SubstancesClass.Jupiter:
                    return GenerateForJupiter(planet.Area);
                default:
                    throw new ArgumentException("Incorect substance type");
            }
        }

        private static Resources GenerateForTerra(double planetArea) {
            double randomResourceMultiplier = GetRandomResourceMultiplier();

            double hydrogen = 
                planetArea * (MASS_OF_TEN_KM_CRUST / FRACTION_OF_HYDROGEN) * randomResourceMultiplier;
            double commonMetals = 
                planetArea * (MASS_OF_TEN_KM_CRUST / FRACTION_OF_COMMON_METALS_TERRA) * randomResourceMultiplier;
            double rareEarthElements = 
                planetArea * (MASS_OF_TEN_KM_CRUST / FRACTION_OF_RARE_METALS_TERRA) * randomResourceMultiplier;

            return new Resources(hydrogen, commonMetals, rareEarthElements);
        }

        private static Resources GenerateForFerria(double planetArea) {
            double randomResourceMultiplier = GetRandomResourceMultiplier();

            double hydrogen = 
                planetArea * (MASS_OF_TEN_KM_CRUST / FRACTION_OF_HYDROGEN) * randomResourceMultiplier;
            double commonMetals = 
                planetArea * (MASS_OF_TEN_KM_CRUST / FRACTION_OF_COMMON_METALS_FERRIA) * randomResourceMultiplier;
            double rareEarthElements = 
                planetArea * (MASS_OF_TEN_KM_CRUST / FRACTION_OF_RARE_METALS_FERRIA) * randomResourceMultiplier;
            
            return new Resources(hydrogen, commonMetals, rareEarthElements);
        }

        private static Resources GenerateForJupiter(double planetArea) {
            double commonMetals = 0;
            double rareEarthElements = 0;

            double randomResourceMultiplier = GetRandomResourceMultiplier();
            double hydrogen = planetArea * GAS_GIANT_HYDROGEN_MULTIPLIER * randomResourceMultiplier;

            return new Resources(hydrogen, commonMetals, rareEarthElements);
        }

        private static double GetRandomResourceMultiplier() {
            return HelperRandomFunctions.GetRandomDouble() * 2;
        }
    }
}
