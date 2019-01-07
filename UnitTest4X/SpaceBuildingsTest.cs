using Logic.Buildings;
using Logic.Resource;
using NUnit.Framework;
using System;

namespace UnitTest4X {
    [TestFixture]
    public class SpaceBuildingsTest {
        [TestCase]
        public void HabitatBuilderOneTurnProgress_NotEnoughResources_NoProgress() {
            HabitatBuilder habitatBuilder = new HabitatBuilder("a");

            double resourceFactor = 0.5;

            Resources neededResources = new Resources(
                resourceFactor * habitatBuilder.CostPerTurn.Hydrogen,
                resourceFactor * habitatBuilder.CostPerTurn.CommonMetals,
                resourceFactor * habitatBuilder.CostPerTurn.RareEarthElements
            );

            habitatBuilder.OneTurnProgress(neededResources);

            Assert.AreEqual(0, habitatBuilder.BuildingProgress);
        }

        [TestCase]
        public void HabitatBuilderOneTurnProgress_EnoughResources_ProgressMade() {
            HabitatBuilder habitatBuilder = new HabitatBuilder("a");

            double resourceFactor = 5;

            Resources neededResources = new Resources(
                resourceFactor * habitatBuilder.CostPerTurn.Hydrogen,
                resourceFactor * habitatBuilder.CostPerTurn.CommonMetals,
                resourceFactor * habitatBuilder.CostPerTurn.RareEarthElements
            );
            for (int i = 0; i < resourceFactor; i++) {
                habitatBuilder.OneTurnProgress(neededResources);
            }
            
            Assert.AreEqual((int)resourceFactor, habitatBuilder.BuildingProgress);
        }

        [TestCase]
        public void HabitatBuilderOneTurnProgress_EnoughResources_HabitatBuilt() {
            HabitatBuilder habitatBuilder = new HabitatBuilder("a");

            double resourceFactor = Habitat.BuildingTime * 2;

            Resources neededResources = new Resources(
                resourceFactor * habitatBuilder.CostPerTurn.Hydrogen,
                resourceFactor * habitatBuilder.CostPerTurn.CommonMetals,
                resourceFactor * habitatBuilder.CostPerTurn.RareEarthElements
            );
            for (int i = 0; i < resourceFactor; i++) {
                habitatBuilder.OneTurnProgress(neededResources);
            }

            Assert.AreEqual(Habitat.BuildingTime, habitatBuilder.BuildingProgress);
        }

        [TestCase]
        public void SystemBuildingsBuildNew_ObjectAddedTwice_CountIsCorrect() {
            HabitatBuilder habitatBuilder = new HabitatBuilder("a");
            HabitatBuilder habitatBuilder2 = new HabitatBuilder("a");

            SystemBuildings systemBuildings = new SystemBuildings();

            systemBuildings.BuildNew(habitatBuilder);
            systemBuildings.BuildNew(habitatBuilder);

            systemBuildings.BuildNew(habitatBuilder2);

            habitatBuilder2.OneTurnProgress(new Resources(double.MaxValue, double.MaxValue, double.MaxValue));
            systemBuildings.BuildNew(habitatBuilder2);

            Assert.AreEqual(2, systemBuildings.InConstructionCount);
        }

        [TestCase]
        public void SystemBuildingsBuildNew_ArgIsNull_ExceptionThrown() {
            SystemBuildings systemBuildings = new SystemBuildings();
            HabitatBuilder habitatBuilder = null;

            Assert.Throws<ArgumentNullException>(() => { systemBuildings.BuildNew(habitatBuilder); });
        }

        [TestCase]
        public void SystemBuildingsBuildNew_ArgsAreValid_Added() {
            SystemBuildings systemBuildings = new SystemBuildings();

            int addedBuildings = 10;

            for (int i = 0; i < addedBuildings; i++) {
                systemBuildings.BuildNew(new HabitatBuilder("a"));
            }
            
            Assert.AreEqual(addedBuildings, systemBuildings.InConstructionCount);
        }

        [TestCase]
        public void SystemBuildingsNextTurn_ArgsAreValid_Built() {
            SystemBuildings systemBuildings = new SystemBuildings();
            Resources resources = new Resources(double.MaxValue, double.MaxValue, double.MaxValue);

            int addedBuildings = 10_000;
            int turns = 30;

            for (int i = 0; i < addedBuildings; i++) {
                systemBuildings.BuildNew(new HabitatBuilder("a"));
            }

            for (int i = 0; i < turns; i++) {
                systemBuildings.NextTurn(resources);
            }

            Assert.AreEqual(addedBuildings, systemBuildings.ExistingCount);
        }
    }
}
