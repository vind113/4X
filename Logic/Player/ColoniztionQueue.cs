using Logic.SpaceObjects;
using System;
using System.Collections.Generic;

namespace Logic.PlayerClasses {
    [Serializable]
    public class ColoniztionQueue {
        private Queue<HabitablePlanet> planetsToColonize = new Queue<HabitablePlanet>();

        public void Add(HabitablePlanet planet) {
            if (planet == null) {
                throw new ArgumentNullException(nameof(planet));
            }

            if (!this.planetsToColonize.Contains(planet)) {
                this.planetsToColonize.Enqueue(planet);
            }
        }

        public void ColonizeWhilePossible(Player player) {
            if (this.planetsToColonize.Count == 0) {
                return;
            }

            while (this.planetsToColonize.Count > 0) {
                ColonizationState state =
                    this.planetsToColonize.Peek().Colonize(player);

                if (state == ColonizationState.NotColonized) {
                    break;
                }
                this.planetsToColonize.Dequeue();
            }
        }
    }
}
