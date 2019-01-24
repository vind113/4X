using Logic.PlayerClasses;
using Logic.SpaceObjects;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTest4X {
    [TestFixture]
    public class StarSystemContainerTest {
        [TestCase]
        public void AddStarSystem_SystemIsNull_ExceptionThrown() {
            Assert.Throws<ArgumentNullException>(() => {
                new StarSystemContainer().AddStarSystem(null);
            });
        }

        [TestCase]
        public void AddStarSystem_SystemIsValid_SystemAdded() {
            var mockSystem = new Mock<StarSystem>("name", new List<Star>(), new List<Planet>()).Object;
            var container = new StarSystemContainer();

            Assert.AreEqual(0, container.StarSystemsCount);

            container.AddStarSystem(mockSystem);

            Assert.AreEqual(1, container.StarSystemsCount);
        }

        [TestCase]
        public void AddStarSystem_SystemWasAddedTwice_SystemAddedOnce() {
            var mockSystem = new Mock<StarSystem>("name", new List<Star>(), new List<Planet>()).Object;
            var container = new StarSystemContainer();

            Assert.AreEqual(0, container.StarSystemsCount);

            container.AddStarSystem(mockSystem);
            container.AddStarSystem(mockSystem);

            Assert.AreEqual(1, container.StarSystemsCount);
        }

        [TestCase]
        public void RemoveStarSystem_SystemIsNull_ExceptionThrown() {
            Assert.Throws<ArgumentNullException>(() => {
                new StarSystemContainer().RemoveStarSystem(null);
            });
        }

        [TestCase]
        public void RemoveStarSystem_SystemIsNotInContainer_NoChanges() {
            var mockInContainer = new Mock<StarSystem>("name", new List<Star>(), new List<Planet>()).Object;
            var mockNotInContainer = new Mock<StarSystem>("name", new List<Star>(), new List<Planet>()).Object;
            var container = new StarSystemContainer();

            Assert.AreEqual(0, container.StarSystemsCount);

            container.AddStarSystem(mockInContainer);

            Assert.AreEqual(1, container.StarSystemsCount);

            container.RemoveStarSystem(mockNotInContainer);

            Assert.AreEqual(1, container.StarSystemsCount);
        }

        [TestCase]
        public void RemoveStarSystem_SystemIsValid_SystemRemoved() {
            var mockSystem = new Mock<StarSystem>("name", new List<Star>(), new List<Planet>()).Object;
            var container = new StarSystemContainer();

            Assert.AreEqual(0, container.StarSystemsCount);

            container.AddStarSystem(mockSystem);

            Assert.AreEqual(1, container.StarSystemsCount);

            container.RemoveStarSystem(mockSystem);

            Assert.AreEqual(0, container.StarSystemsCount);
        }
    }
}
