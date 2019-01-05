using Logic.SpaceObjects;
using Logic.SupportClasses;
using System;

namespace Logic.PlayerClasses {
    public static class Discovery {
        private const long PERSPECTIVE_COLONY_MIN_POPULATION = 10_000_000_000;
        //с такой вероятностью каждый ход будет открываться новая система
        //возможно добавить зависимость от уровня технологий
        //оптимальное значение - 0.15
        const double DISCOVERY_PROBABILITY = 0.15;

        public static void TryToDiscoverNewStarSystem(bool isAutoColonizationEnabled, Player player) {
            if (HelperRandomFunctions.ProbableBool(DISCOVERY_PROBABILITY)) {
                DiscoverNewStarSystem(isAutoColonizationEnabled, player);
            }
        }

        private static void DiscoverNewStarSystem(bool isAutoColonizationEnabled, Player player) {
            int maxSystemsToGenerate = 0;
            int systemsToGenerate = 0;

            //checked {
            maxSystemsToGenerate = (int)((Math.Sqrt(player.StarSystemsCount)) / 2);
            systemsToGenerate = HelperRandomFunctions.GetRandomInt(1, maxSystemsToGenerate + 1);
            //}

            for (int index = 0; index < systemsToGenerate; index++) {
                StarSystem generatedSystem =
                    StarSystemFactory.GetStarSystem($"System {player.StarSystemsCount + 1} #{index}");

                if (isAutoColonizationEnabled) {
                    ColonizeSystem(player, generatedSystem);
                }

                player.AddStarSystem(generatedSystem);
            }
        }

        private static void ColonizeSystem(Player player, StarSystem generatedSystem) {
            foreach (var planet in generatedSystem.SystemPlanets) {
                if (planet.Population.MaxValue >= PERSPECTIVE_COLONY_MIN_POPULATION) planet.Colonize(player);
            }
        }
    }
}
