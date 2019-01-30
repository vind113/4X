using Logic.SpaceObjects;
using System.Collections.Generic;

namespace Logic.PlayerClasses {
    class StarSystemsExplorer {
        private Player owner;

        public StarSystemsExplorer(Player player) {
            this.owner = player;
        }

        public void DiscoverNewSystems() {
            IList<StarSystem> generatedSystems = new List<StarSystem>();
            if (owner.IsDiscoveringNewStarSystems) {
                generatedSystems = Discovery.TryToFindNewStarSystems(owner.StarSystemsCount);
            }

            foreach (var system in generatedSystems) {
                owner.AddStarSystem(system);
                if (owner.IsAutoColonizationEnabled) {
                    ColonizeSystem(system);
                }
            }
        }

        private void ColonizeSystem(StarSystem system) {
            foreach (var planet in system.SystemHabitablePlanets) {
                if (planet.Colonize(owner.Ships.GetColonizer()) == ColonizationState.NotColonized) {
                    owner.AddToColonizationQueue(planet);
                } 
            }
        }
    }
}
