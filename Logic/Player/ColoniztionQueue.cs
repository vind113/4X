using Logic.Resource;
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

        public void ColonizeWhilePossible(Ships ships, Resources resources) {
            if (ships == null) {
                throw new ArgumentNullException(nameof(ships));
            }

            if (resources == null) {
                throw new ArgumentNullException(nameof(resources));
            }

            while (this.planetsToColonize.Count > 0) {
                ColonizationState state =
                    this.planetsToColonize.Peek().Colonize(ships.GetColonizer(resources));

                if (state == ColonizationState.NotColonized) {
                    break;
                }
                this.planetsToColonize.Dequeue();
            }
        }
    }
}
