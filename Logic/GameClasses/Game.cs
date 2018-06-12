using Logic.PlayerClasses;
using Logic.Resourse;
using Logic.Space_Objects;

namespace Logic.GameClasses {
    public static class Game {
        private static Player player;
        private static CurrentDate currentDate;

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

        public static void NextTurn() {
            currentDate.NextTurn();
            foreach(StarSystem system in player.StarSystems) {
                system.NextTurn(player);
            }
        }
       
        public static void ResetDate() {
            currentDate = new CurrentDate();
        }

        public static void GameReInit() {
            player = new Player();
            currentDate = new CurrentDate();
        }
    }
}
