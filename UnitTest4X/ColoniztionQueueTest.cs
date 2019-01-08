using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnitTest4X.Mocks;
using Logic.Resource;
using Logic.SpaceObjects;
using Logic.PlayerClasses;

namespace UnitTest4X {
    [TestFixture]
    public class ColoniztionQueueTest {
        [TestCase]
        public void TryToColonizeQueue_QueueCountIsZero_ImmediateReturn() {
            Player player = new Player();

            Assert.DoesNotThrow(new TestDelegate(player.TryToColonizeQueue));
        }

        [TestCase]
        public void TryToColonizeQueue_QueueCountIsGreaterThanZero() {
            Player player = new Player();
            int planetsToColonize = 255;

            player.OwnedResources = new Resources(Double.MaxValue, Double.MaxValue, Double.MaxValue);

            List<Planet> planetList = new List<Planet>();

            for (int i = 0; i < planetsToColonize; i++) {
                planetList.Add(PlanetFactory.GetHabitablePlanet("a", new PlanetType(TemperatureClass.Temperate, VolatilesClass.Marine, SubstancesClass.Terra)));
            }

            player.AddStarSystem(new StarSystem("system", new List<Star>(), planetList));

            foreach (var system in player.StarSystems) {
                foreach (var planet in system.SystemHabitablePlanets) {
                    player.AddToColonizationQueue(planet);
                }
            }

            player.TryToColonizeQueue();

            Assert.AreEqual(planetsToColonize + player.StarSystems[0].HabitablePlanetsCount, player.ColonizedPlanets);
        }
    }
}
