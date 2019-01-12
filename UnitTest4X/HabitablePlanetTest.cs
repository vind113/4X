using Logic.PlayerClasses;
using Logic.SpaceObjects;
using Moq;
using NUnit.Framework;

namespace UnitTest4X {
    [TestFixture]
    public class HabitablePlanetTest {
        [TestCase]
        public void Colonize_PlanetColonized_ColonizedStateReturned() {
            const int expectedPopulation = 100_000;
            var planet = new HabitablePlanet("a", 10_000,
                    new PlanetType(TemperatureClass.Temperate, VolatilesClass.Marine, SubstancesClass.Terra), expectedPopulation);

            var actual = planet.Colonize(null);

            Assert.AreEqual(ColonizationState.Colonized, actual);
            Assert.AreEqual(expectedPopulation, planet.Population.Value);
        }

        [TestCase]
        public void Colonize_ColonizerPassed_PlanetColonized() {
            var planet = new HabitablePlanet("a", 10_000,
                new PlanetType(TemperatureClass.Temperate, VolatilesClass.Marine, SubstancesClass.Terra), 0);

            var actual = planet.Colonize(new Mock<Colonizer>().Object);

            Assert.AreEqual(ColonizationState.Colonized, actual);
            Assert.AreEqual(Colonizer.Colonists, planet.Population.Value);
            
        }

        [TestCase]
        public void Colonize_ColonizerIsNull_NotColonizedStateReturned() {
            var planet = new HabitablePlanet("a", 10_000,
                new PlanetType(TemperatureClass.Temperate, VolatilesClass.Marine, SubstancesClass.Terra), 0);

            var actual = planet.Colonize(null);

            Assert.AreEqual(ColonizationState.NotColonized, actual);
            Assert.AreEqual(0, planet.Population.Value);
        }
    }
}
