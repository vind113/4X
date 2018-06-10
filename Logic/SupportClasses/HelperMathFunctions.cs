using System;

namespace Logic.SupportClasses {
    public class HelperMathFunctions {
        public static double SphereArea(double radius) {
            return 4 * Math.PI * Math.Pow(radius, 2);
        }
    }

    public static class HelperRandomFunctions {
        private static Random randomizer;

        static HelperRandomFunctions() {
            randomizer = new Random();
        }

        public static double GetRandomDouble() {
            return randomizer.NextDouble();
        }

        public static int GetRandomInt(int minValue, int maxValue) {
            return randomizer.Next(minValue, maxValue);
        }
    }
}
