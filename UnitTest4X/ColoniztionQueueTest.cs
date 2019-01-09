using System;
using System.Collections.Generic;
using NUnit.Framework;
using Logic.Resource;
using Logic.SpaceObjects;
using Logic.PlayerClasses;
using Autofac.Extras.Moq;

namespace UnitTest4X {
    [TestFixture]
    public class ColoniztionQueueTest {
        [TestCase]
        public void TryToColonizeQueue_QueueCountIsZero_ImmediateReturn() {
            using (var mock = AutoMock.GetLoose()) {
                ColoniztionQueue coloniztionQueue = new ColoniztionQueue();

                Assert.DoesNotThrow(() => {
                    coloniztionQueue.ColonizeWhilePossible(new Ships(), new Resources());
                });
            }
        }

        [TestCase]
        public void TryToColonizeQueue_QueueCountIsGreaterThanZero() {
            using (var mock = AutoMock.GetLoose()) {
                ColoniztionQueue coloniztionQueue = new ColoniztionQueue();
                int planetsToColonize = 1000;

                List<HabitablePlanet> planetList = new List<HabitablePlanet>();

                for (int i = 0; i < planetsToColonize; i++) {
                    planetList.Add(new HabitablePlanet("a", 10_000,
                        new PlanetType(TemperatureClass.Temperate, VolatilesClass.Marine, SubstancesClass.Terra), 0));
                }

                foreach (var planet in planetList) {
                    coloniztionQueue.Add(planet);
                }

                coloniztionQueue.ColonizeWhilePossible(
                    new Ships(), new Resources(Double.MaxValue, Double.MaxValue, Double.MaxValue));

                int colonizedPlanets = 0;
                foreach(var planet in planetList) {
                    if (planet.IsColonized) {
                        colonizedPlanets++;
                    }
                }

                Assert.AreEqual(planetsToColonize, colonizedPlanets);
            }
        }
    }
}
