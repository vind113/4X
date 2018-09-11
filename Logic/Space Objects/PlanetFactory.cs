using Logic.SupportClasses;

namespace Logic.SpaceObjects {
    public class PlanetFactory {
        private const int smallPlanetMinRadius = 3_000;
        private const int mediumPlanetMinRadius = 9_000;
        private const int bigPlanetMinRadius = 11_000;
        private const int bigPlanetMaxRadius = 13_000;

        private const int smallGasPlanetMinRadius = 20_000;
        private const int mediumGasPlanetMinRadius = 100_000;
        private const int bigGasPlanetMinRadius = 110_000;
        private const int bigGasPlanetMaxRadius = 150_000;

        /// <summary>
        ///     Генерирует планету с заданым именем и типом
        /// </summary>
        /// <param name="name">
        ///     Имя планеты
        /// </param>
        /// <param name="planetType">
        ///     Тип планеты(пустынная, океаническая, газовый гигант и т.д)
        /// </param>
        /// <returns></returns>
        public static Planet GetPlanet(string name, PlanetType planetType) {
            string planetName = name;
            long population = 0;
            double radius = 0;

            if (planetType.SubstancesClass == SubstancesClass.Jupiter) {
                radius = GasGiantRadiusGeneration();
            }
            else {
                radius = RockyPlanetRadiusGeneration();
            }

            return new Planet(planetName, radius, planetType, population);
        }

        private static double RockyPlanetRadiusGeneration() {
            double radius;

            if (HelperRandomFunctions.PercentProbableBool(8)) {
                radius = (double)HelperRandomFunctions.GetRandomInt(bigPlanetMinRadius, bigPlanetMaxRadius);
            }
            else if (HelperRandomFunctions.PercentProbableBool(15)) {
                radius = (double)HelperRandomFunctions.GetRandomInt(mediumPlanetMinRadius, bigPlanetMinRadius);
            }
            else {
                radius = (double)HelperRandomFunctions.GetRandomInt(smallPlanetMinRadius, mediumPlanetMinRadius);
            }

            return radius;
        }

        private static double GasGiantRadiusGeneration() {
            double radius;
            if (HelperRandomFunctions.PercentProbableBool(10)) {
                radius = (double)HelperRandomFunctions.GetRandomInt(bigGasPlanetMinRadius, bigGasPlanetMaxRadius);
            }
            else if (HelperRandomFunctions.PercentProbableBool(20)) {
                radius = (double)HelperRandomFunctions.GetRandomInt(mediumGasPlanetMinRadius, bigGasPlanetMinRadius);
            }
            else {
                radius = (double)HelperRandomFunctions.GetRandomInt(smallGasPlanetMinRadius, mediumGasPlanetMinRadius);
            }
            return radius;
        }

    }
}
