using Logic.PlayerClasses;
using Logic.Resourse;
using NUnit.Framework;
using System;
using UnitTest4X.Mocks;

namespace UnitTest4X {
    [TestFixture]
    public class CivilProductionTest {
        [TestCase]
        public void SustainPopulationNeeds_PlayerIsNull_ExceptionThrown() {
            Assert.Throws<ArgumentNullException>(() => CivilProduction.SustainPopulationNeeds(null));
        }

        [TestCase]
        public void SustainPopulationNeeds_PopulationLessEqualZero_ResoursesDidNotChange() {
            IPlayer player = new PlayerMock() {
                TotalPopulation = 0,
                OwnedResourses = new Resourses(1, 1, 1)
            };

            CivilProduction.SustainPopulationNeeds(player);

            Assert.AreEqual(1, player.OwnedResourses.Hydrogen);
            Assert.AreEqual(1, player.OwnedResourses.CommonMetals);
            Assert.AreEqual(1, player.OwnedResourses.RareEarthElements);
        }

        [TestCase]
        public void SustainPopulationNeeds_CorrectCondtions_ResoursesDecreased() {
            const int resoursesAmount = 10000;
            const int population = 100_000;
            IPlayer player = new PlayerMock() {
                TotalPopulation = population,
                OwnedResourses = new Resourses(resoursesAmount, resoursesAmount, resoursesAmount)
            };

            CivilProduction.SustainPopulationNeeds(player);

            Assert.AreEqual(resoursesAmount - (CivilProduction.HYDROGEN_PER_PERSON * population),
                player.OwnedResourses.Hydrogen);
            Assert.AreEqual(resoursesAmount - (CivilProduction.COMMON_METALS_PER_PERSON * population),
                player.OwnedResourses.CommonMetals);
            Assert.AreEqual(resoursesAmount - (CivilProduction.RARE_METALS_PER_PERSON * population),
                player.OwnedResourses.RareEarthElements);
        }

        [TestCase]
        public void SustainPopulationNeeds_NotEnoughResourses_ResoursesDecreasedToZero() {
            const int resoursesAmount = 4_999;
            const int population = 100_000;

            IPlayer player = new PlayerMock() {
                TotalPopulation = population,
                OwnedResourses = new Resourses(resoursesAmount, resoursesAmount, resoursesAmount)
            };

            CivilProduction.SustainPopulationNeeds(player);

            Assert.AreEqual(0, player.OwnedResourses.Hydrogen);
            Assert.AreEqual(0, player.OwnedResourses.CommonMetals);
            Assert.AreEqual(0, player.OwnedResourses.RareEarthElements);
        }
    }
}
