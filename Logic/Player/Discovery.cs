using Logic.SpaceObjects;
using Logic.SupportClasses;
using System;

namespace Logic.PlayerClasses {
    public class Discovery {
        public static void NewStarSystem(bool isAutoColonizationEnabled, Player player) {
            //с такой вероятностью каждый ход будет открываться новая система
            //возможно добавить зависимость от уровня технологий
            //оптимальное значение - 0.15
            const double discoveryProbability = 0.15;

            if (HelperRandomFunctions.ProbableBool(discoveryProbability)) {
                int maxSystemsToGenerate = 0;
                int systemsToGenerate = 0;
                
                checked {
                    maxSystemsToGenerate = (int)((Math.Sqrt(player.StarSystemsCount)) / 2);
                    systemsToGenerate = HelperRandomFunctions.GetRandomInt(1, maxSystemsToGenerate + 1);
                }

                StarSystem generatedSystem = null;

                for (int index = 0; index < systemsToGenerate; index++) {
                    generatedSystem = StarSystemFactory.GetStarSystem($"System {player.StarSystemsCount + 1} #{index}");

                    if (isAutoColonizationEnabled) {
                        foreach (var planet in generatedSystem.SystemPlanets) {
                            planet.Colonize(player);
                        }
                    }

                    player.AddStarSystem(generatedSystem);
                }
            }
        }
    }
}
