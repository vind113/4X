using Logic.Resource;
using Logic.SpaceObjects;
using System;
using System.Collections.Generic;

namespace Logic.PlayerClasses {
    [Serializable]
    public class ColoniztionQueue {
        private Queue<IHabitablePlanet> planetsToColonize = new Queue<IHabitablePlanet>();

        public int PlanetsInQueue { get => planetsToColonize.Count; }

        public ColoniztionQueue() {

        }

        public ColoniztionQueue(IEnumerable<IHabitablePlanet> planets) {
            planetsToColonize = new Queue<IHabitablePlanet>(planets);
        }

        public void Add(IHabitablePlanet planet) {
            if (planet == null) {
                throw new ArgumentNullException(nameof(planet));
            }

            if (!this.planetsToColonize.Contains(planet)) {
                this.planetsToColonize.Enqueue(planet);
            }
        }

        public void ColonizeWhilePossible(IShips ships) {
            if (ships == null) {
                throw new ArgumentNullException(nameof(ships));
            }

            while (this.planetsToColonize.Count > 0) {
                Colonizer colonizer = ships.GetColonizer();
                ColonizationState state =
                    this.planetsToColonize.Peek().Colonize(colonizer);

                if (state == ColonizationState.NotColonized
                 || state == ColonizationState.Unknown) {
                    break;
                }
                else {
                    this.planetsToColonize.Dequeue();
                }
            }
        }
    }
}
