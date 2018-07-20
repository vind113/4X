using Logic.PlayerClasses;
using Logic.Resourse;
using Logic.Space_Objects;
using Logic.SupportClasses;

using System;
using System.Collections.Generic;

namespace Logic.GameClasses {
    public class Game {
        private Player player;
        private CurrentDate currentDate;
        private bool isAutoColonizationEnabled = false;

        private string lastGameMessage;

        public Game() {
            player = new Player();
            currentDate = new CurrentDate();
            player.StarSystems.Add(StarSystem.GetSolarSystem());
        }

        #region Properties
        public int GameTurn { get => currentDate.Turn; }
        public string GameDate { get => currentDate.Date; }
        public Player Player { get => player; }

        /*public List<StarSystem> PlayerStarSystems { get => player.StarSystems; }

        public Resourses PlayerResourses { get=> player.OwnedResourses}*/

        public bool IsAutoColonizationEnabled {
            get => isAutoColonizationEnabled;
            set => isAutoColonizationEnabled = value;
        }
        #endregion

        #region Next Turn Functionality
        public void NextTurn() {
            currentDate.NextTurn();
            DiscoverNewStarSystem();

            foreach (StarSystem system in player.StarSystems) {
                system.NextTurn(player);
            }
            SetPlayerCitizenHubCapacity();
        }

        private void DiscoverNewStarSystem() {
            //с такой вероятностью каждый ход будет открываться новая система
            //возможно добавить зависимость от уровня технологий
            //оптимальное значение - 0.15
            double discoveryProbability = 0.15; 
            
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

                    player.OwnedStars += generatedSystem.SystemStars.Count;
                    player.OwnedPlanets += generatedSystem.SystemPlanets.Count;

                    if (isAutoColonizationEnabled) {
                        foreach (var planet in generatedSystem.SystemPlanets) {
                            planet.Colonize(player);
                        }
                    }

                    player.StarSystems.Add(generatedSystem);
                }
            }
        }

        private void SetPlayerCitizenHubCapacity() {
            double newHubCapacity = Math.Ceiling(player.TotalPopulation / 1000);
            
            if (newHubCapacity > player.PlayerCitizenHub.CitizensInHub) {
                player.PlayerCitizenHub.MaximumCount = newHubCapacity;
            }
        }

        #endregion

        public void ResetDate() {
            currentDate = new CurrentDate();
        }

        public void ResetGame() {
            player = new Player();
            currentDate = new CurrentDate();
        }
    }
}
