using System;

namespace Logic.PlayerClasses {
    public enum ColonizationMode {
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

        public bool DetermineAutoColonizationState(ColonizationMode mode) {
            switch (mode) {
                case ColonizationMode.None:
                    return false;
                case ColonizationMode.Auto:
                    return IsPopulationDensitySufficent();
                case ColonizationMode.All:
                    return true;
                default:
                    throw new ArgumentException("Incorrect colonization mode");
            }
        }

        private bool IsPopulationDensitySufficent() {
            return (this.player.Population / this.player.ColonizedPlanets) > MinimumPlanetPopulation;
        }
    }
}