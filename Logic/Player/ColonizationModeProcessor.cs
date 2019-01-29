using System;

namespace Logic.PlayerClasses {
    public enum ColonizatonModes {
        None,
        Auto,
        All
    }

    public class ColonizationModeProcessor {
        private const long MinimumPlanetPopulation = 5_000_000_000; 

        private readonly IPlayer player;

        public ColonizationModeProcessor(IPlayer player) {
            if (player == null) {
                throw new ArgumentNullException(nameof(player));
            }

            this.player = player;
        }

        public bool GetAutoColoniztionState(ColonizatonModes mode) {
            switch (mode) {
                case ColonizatonModes.None:
                    return false;
                case ColonizatonModes.Auto:
                    return IsPopulationDensitySufficent();
                case ColonizatonModes.All:
                    return true;
                default:
                    throw new ArgumentException("Incorrect colonization mode");
            }
        }

        private bool IsPopulationDensitySufficent() {
            return this.player.Population / this.player.ColonizedPlanets > MinimumPlanetPopulation;
        }
    }
}