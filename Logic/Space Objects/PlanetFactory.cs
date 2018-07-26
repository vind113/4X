using Logic.SupportClasses;

namespace Logic.Space_Objects {
    public class PlanetFactory {
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
        public static Planet GeneratePlanet(string name, PlanetTypeValue planetType) {
            string planetName = name;
            double population = 0;
            double radius = 0;

            if (planetType == PlanetTypeValue.GasGiant) {
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
                radius = (double)HelperRandomFunctions.GetRandomInt(11_000, 13_000);
            }
            else if (HelperRandomFunctions.PercentProbableBool(15)) {
                radius = (double)HelperRandomFunctions.GetRandomInt(9_000, 11_000);
            }
            else {
                radius = (double)HelperRandomFunctions.GetRandomInt(3_000, 9_000);
            }

            return radius;
        }

        private static double GasGiantRadiusGeneration() {
            double radius;
            if (HelperRandomFunctions.PercentProbableBool(10)) {
                radius = (double)HelperRandomFunctions.GetRandomInt(110_000, 150_000);
            }
            else if (HelperRandomFunctions.PercentProbableBool(20)) {
                radius = (double)HelperRandomFunctions.GetRandomInt(100_000, 110_000);
            }
            else {
                radius = (double)HelperRandomFunctions.GetRandomInt(20_000, 100_000);
            }
            return radius;
        }

    }
}
