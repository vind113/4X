using Logic.Buildings;
using Logic.Resourse;
using NUnit.Framework;
using System;

namespace UnitTest4X {
    [TestFixture]
    public class SpaceBuildingsTest {
        [TestCase]
        public void HabitatBuilderOneTurnProgress_NotEnoughResourses_NoProgress() {
            HabitatBuilder habitatBuilder = new HabitatBuilder("a");

            double resourseFactor = 0.5;

            Resourses neededResourses = new Resourses(
                resourseFactor * habitatBuilder.CostPerTurn.Hydrogen,
                resourseFactor * habitatBuilder.CostPerTurn.CommonMetals,
                resourseFactor * habitatBuilder.CostPerTurn.RareEarthElements
            );

            habitatBuilder.OneTurnProgress(neededResourses);

            Assert.AreEqual(0, habitatBuilder.BuildingProgress);
        }

        [TestCase]
        public void HabitatBuilderOneTurnProgress_EnoughResourses_ProgressMade() {
            HabitatBuilder habitatBuilder = new HabitatBuilder("a");

            double resourseFactor = 5;

            Resourses neededResourses = new Resourses(
                resourseFactor * habitatBuilder.CostPerTurn.Hydrogen,
                resourseFactor * habitatBuilder.CostPerTurn.CommonMetals,
                resourseFactor * habitatBuilder.CostPerTurn.RareEarthElements
            );
            for (int i = 0; i < resourseFactor; i++) {
                habitatBuilder.OneTurnProgress(neededResourses);
            }
            
            Assert.AreEqual((int)resourseFactor, habitatBuilder.BuildingProgress);
        }

        [TestCase]
        public void HabitatBuilderOneTurnProgress_EnoughResourses_HabitatBuilt() {
            HabitatBuilder habitatBuilder = new HabitatBuilder("a");

            double resourseFactor = Habitat.BuildingTime * 2;

            Resourses neededResourses = new Resourses(
                resourseFactor * habitatBuilder.CostPerTurn.Hydrogen,
                resourseFactor * habitatBuilder.CostPerTurn.CommonMetals,
                resourseFactor * habitatBuilder.CostPerTurn.RareEarthElements
            );
            for (int i = 0; i < resourseFactor; i++) {
                habitatBuilder.OneTurnProgress(neededResourses);
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

            habitatBuilder2.OneTurnProgress(new Resourses(double.MaxValue, double.MaxValue, double.MaxValue));
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
            Resourses resourses = new Resourses(double.MaxValue, double.MaxValue, double.MaxValue);

            int addedBuildings = 10_000;
            int turns = 30;

            for (int i = 0; i < addedBuildings; i++) {
                systemBuildings.BuildNew(new HabitatBuilder("a"));
            }

            for (int i = 0; i < turns; i++) {
                systemBuildings.NextTurn(resourses);
            }

            Assert.AreEqual(addedBuildings, systemBuildings.ExistingCount);
        }
    }
}
