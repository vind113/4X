using Logic.SpaceObjects;
using Logic.SupportClasses;
using System;
using System.Collections.Generic;

namespace Logic.PlayerClasses {
    public static class Discovery {
        //с такой вероятностью каждый ход будет открываться новая система
        //возможно добавить зависимость от уровня технологий
        //оптимальное значение - 0.15
        private const double DiscoveryProbability = 0.15;

        public static IList<StarSystem> TryToDiscoverNewStarSystem(int discoveredSystemsCount) {
            if(discoveredSystemsCount <= 0) {
                throw new ArgumentOutOfRangeException("discoveredSystems count must be greater than zero");
            }

            IList<StarSystem> generatedSystems = new List<StarSystem>();

            if (HelperRandomFunctions.ProbableBool(DiscoveryProbability)) {
                generatedSystems = DiscoverNewStarSystem(discoveredSystemsCount);
            }

            return generatedSystems;
        }

        private static IList<StarSystem> DiscoverNewStarSystem(int discoveredSystemsCount) {
            int maxSystemsToGenerate = 0;
            int systemsToGenerate = 0;
            IList<StarSystem> generatedSystems = new List<StarSystem>();

            maxSystemsToGenerate = (int)((Math.Sqrt(discoveredSystemsCount)) / 2);
            systemsToGenerate = HelperRandomFunctions.GetRandomInt(1, maxSystemsToGenerate + 1);

            for (int index = 0; index < systemsToGenerate; index++) {
                StarSystem generatedSystem =
                    StarSystemFactory.GetStarSystem($"System {discoveredSystemsCount + 1} #{index}");

                generatedSystems.Add(generatedSystem);
            }

            return generatedSystems;
        }
    }
}
