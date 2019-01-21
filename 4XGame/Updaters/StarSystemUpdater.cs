using Logic.PlayerClasses;

namespace _4XGame.Updaters {
    public class PlayerInfoUpdater {
        private Player Player { get; }

        public PlayerInfoUpdater(Player player) {
            this.Player = player;
        }

        public void UpdatePopulation() {
            foreach (var system in Player.StarSystems) {
                system.UpdatePopulation();
            }
        }
    }
}
