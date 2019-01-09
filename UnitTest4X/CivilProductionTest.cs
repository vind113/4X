using Logic.PlayerClasses;
using Logic.Resource;
using NUnit.Framework;
using System;

namespace UnitTest4X {
    [TestFixture]
    public class CivilProductionTest {
        [TestCase]
        public void SustainPopulationNeeds_ResourceIsNull_ExceptionThrown() {
            Assert.Throws<ArgumentNullException>(() => CivilProduction.SustainPopulationNeeds(1, null));
        }

        [TestCase]
        public void SustainPopulationNeeds_PopulationLessEqualZero_ResourcesDidNotChange() {
            Resources from = new Resources(1, 1, 1);
            CivilProduction.SustainPopulationNeeds(0, from);

            Assert.AreEqual(1, from.Hydrogen);
            Assert.AreEqual(1, from.CommonMetals);
            Assert.AreEqual(1, from.RareEarthElements);
        }

        [TestCase]
        public void SustainPopulationNeeds_CorrectCondtions_ResourcesDecreased() {
            const int resourcesAmount = 10000;
            const int population = 100_000;

            Resources from = new Resources(resourcesAmount, resourcesAmount, resourcesAmount);
            CivilProduction.SustainPopulationNeeds(100_000, from);

            Assert.AreEqual(resourcesAmount - (CivilProduction.HYDROGEN_PER_PERSON * population),
                from.Hydrogen);
            Assert.AreEqual(resourcesAmount - (CivilProduction.COMMON_METALS_PER_PERSON * population),
                from.CommonMetals);
            Assert.AreEqual(resourcesAmount - (CivilProduction.RARE_METALS_PER_PERSON * population),
                from.RareEarthElements);
        }

        [TestCase]
        public void SustainPopulationNeeds_NotEnoughResources_ResourcesDecreasedToZero() {
            const int resourcesAmount = 4999;
            const int population = 100_000;

            Resources from = new Resources(resourcesAmount, resourcesAmount, resourcesAmount);
            CivilProduction.SustainPopulationNeeds(population, from);

            Assert.AreEqual(0, from.Hydrogen);
            Assert.AreEqual(0, from.CommonMetals);
            Assert.AreEqual(0, from.RareEarthElements);
        }
    }
}
