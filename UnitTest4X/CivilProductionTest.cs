using Logic.PlayerClasses;
using Logic.Resource;
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
        public void SustainPopulationNeeds_PopulationLessEqualZero_ResourcesDidNotChange() {
            IPlayer player = new PlayerMock() {
                TotalPopulation = 0,
                OwnedResources = new Resources(1, 1, 1)
            };

            CivilProduction.SustainPopulationNeeds(player);

            Assert.AreEqual(1, player.OwnedResources.Hydrogen);
            Assert.AreEqual(1, player.OwnedResources.CommonMetals);
            Assert.AreEqual(1, player.OwnedResources.RareEarthElements);
        }

        [TestCase]
        public void SustainPopulationNeeds_CorrectCondtions_ResourcesDecreased() {
            const int resourcesAmount = 10000;
            const int population = 100_000;
            IPlayer player = new PlayerMock() {
                TotalPopulation = population,
                OwnedResources = new Resources(resourcesAmount, resourcesAmount, resourcesAmount)
            };

            CivilProduction.SustainPopulationNeeds(player);

            Assert.AreEqual(resourcesAmount - (CivilProduction.HYDROGEN_PER_PERSON * population),
                player.OwnedResources.Hydrogen);
            Assert.AreEqual(resourcesAmount - (CivilProduction.COMMON_METALS_PER_PERSON * population),
                player.OwnedResources.CommonMetals);
            Assert.AreEqual(resourcesAmount - (CivilProduction.RARE_METALS_PER_PERSON * population),
                player.OwnedResources.RareEarthElements);
        }

        [TestCase]
        public void SustainPopulationNeeds_NotEnoughResources_ResourcesDecreasedToZero() {
            const int resourcesAmount = 4_999;
            const int population = 100_000;

            IPlayer player = new PlayerMock() {
                TotalPopulation = population,
                OwnedResources = new Resources(resourcesAmount, resourcesAmount, resourcesAmount)
            };

            CivilProduction.SustainPopulationNeeds(player);

            Assert.AreEqual(0, player.OwnedResources.Hydrogen);
            Assert.AreEqual(0, player.OwnedResources.CommonMetals);
            Assert.AreEqual(0, player.OwnedResources.RareEarthElements);
        }
    }
}
