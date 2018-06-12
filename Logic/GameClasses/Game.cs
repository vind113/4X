using Logic.PlayerClasses;
using Logic.Resourse;
using Logic.Space_Objects;

namespace Logic.GameClasses {
    public static class Game {
        private static Player player;
        private static CurrentDate currentDate;

        private static string lastGameMessage;

        static Game() {
            player = new Player();
            currentDate = new CurrentDate();
            player.StarSystems.Add(StarSystem.GetSolarSystem());
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
        #endregion

        #region Next Turn Functionality
        public static void NextTurn() {
            currentDate.NextTurn();
            foreach (StarSystem system in player.StarSystems) {
                system.NextTurn(player);
            }
            SetPlayerCitizenHubCapacity();
        }

        private static void SetPlayerCitizenHubCapacity() {
            double newHubCapacity = player.TotalPopulation / 1000;
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
