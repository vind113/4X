using Logic.PlayerClasses;
using Logic.Resourse;
using Logic.Space_Objects;
using Logic.SupportClasses;

using System;

namespace Logic.GameClasses {
    public static class Game {
        private static Player player;
        private static CurrentDate currentDate;
        private static bool isAutoColonizationEnabled = false;

        private static string lastGameMessage;

        static Game() {
            player = new Player();
            currentDate = new CurrentDate();
            player.StarSystems.Add(StarSystem.GetSolarSystem());
        }

        #region Properties
        public static int GameTurn {
            get => currentDate.Turn;
        }

        public static string GameDate {
            get => currentDate.Date;
        }

        public static Player Player {
            get => player;
        }
        public static bool IsAutoColonizationEnabled {
            get => isAutoColonizationEnabled;
            set => isAutoColonizationEnabled = value;
        }
        #endregion

        #region Next Turn Functionality
        public static void NextTurn() {
            currentDate.NextTurn();
            DiscoverNewStarSystem();

            foreach (StarSystem system in player.StarSystems) {
                system.NextTurn(player);
            }
            SetPlayerCitizenHubCapacity();
        }

        private static void DiscoverNewStarSystem() {
            //с такой вероятностью каждый ход будет открываться новая система
            //возможно добавить зависимость от уровня технологий
            //оптимальное значение - 0.15
            double discoveryProbability = 1; 
            
            if (HelperRandomFunctions.ProbableBool(discoveryProbability)) {
                int maxSystemsToGenerate = 0;
                int systemsToGenerate = 0;
                StarSystem generatedSystem = null;
                checked {
                    maxSystemsToGenerate = (int)((Math.Sqrt(player.StarSystems.Count)) / 2);
                    systemsToGenerate = HelperRandomFunctions.GetRandomInt(1, maxSystemsToGenerate + 1);
                }
                for (int index = 0; index < systemsToGenerate; index++) {
                    //player.StarSystems.Add(StarSystem.GenerateSystem($"System {currentDate.Date}-{index}"));
                    generatedSystem = StarSystem.GenerateSystem($"System {currentDate.Date}-{index}");

                    if (isAutoColonizationEnabled) {
                        foreach (var planet in generatedSystem.SystemPlanets) {
                            planet.Colonize(player);
                        }
                    }

                    player.StarSystems.Add(generatedSystem);
                }
            }
        }

        private static void SetPlayerCitizenHubCapacity() {
            double newHubCapacity = Math.Ceiling(player.TotalPopulation / 1000);
            
            if (newHubCapacity > player.PlayerCitizenHub.CitizensInHub) {
                player.PlayerCitizenHub.MaximumCount = newHubCapacity;
            }
        }

        #endregion

        public static void ResetDate() {
            currentDate = new CurrentDate();
        }

        public static void GameReInit() {
            player = new Player();
            currentDate = new CurrentDate();
        }
    }
}
