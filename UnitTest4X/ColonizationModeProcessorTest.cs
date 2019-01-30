using Logic.PlayerClasses;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest4X {
    [TestFixture]
    public class ColonizationModeProcessorTest {
        [TestCase]
        public void DetermineAutoColonizationState_ModeIsNone_ReturnsFalse() {
            var playerMock = new Mock<IPlayer>();
            var modeDeifner = new ColonizationModeProcessor(playerMock.Object);
            Assert.IsFalse(modeDeifner.DetermineAutoColonizationState(ColonizationMode.None));
        }

        [TestCase]
        public void DetermineAutoColonizationState_ModeIsAll_ReturnsTrue() {
            var playerMock = new Mock<IPlayer>();
            var modeDeifner = new ColonizationModeProcessor(playerMock.Object);
            Assert.IsTrue(modeDeifner.DetermineAutoColonizationState(ColonizationMode.All));
        }

        [TestCase]
        public void DetermineAutoColonizationState_ModeIsAutoAndDensityIsOK_ReturnsTrue() {
            var playerMock = new Mock<IPlayer>();

            const long population = 6_000_000_000;
            const int planetCount = 1;

            playerMock.SetupGet(x => x.Population).Returns(population);
            playerMock.SetupGet(x => x.ColonizedPlanets).Returns(planetCount);

            var modeDefiner = new ColonizationModeProcessor(playerMock.Object);

            Assert.IsTrue(modeDefiner.DetermineAutoColonizationState(ColonizationMode.Auto));
        }

        [TestCase]
        public void DetermineAutoColonizationState_ModeIsAutoAndDensityIsNotOk_ReturnsFalse() {
            var playerMock = new Mock<IPlayer>();

            const long population = 9_000_000_000;
            const int planetCount = 2;

            playerMock.SetupGet(x => x.Population).Returns(population);
            playerMock.SetupGet(x => x.ColonizedPlanets).Returns(planetCount);

            var modeDefiner = new ColonizationModeProcessor(playerMock.Object);

            Assert.IsFalse(modeDefiner.DetermineAutoColonizationState(ColonizationMode.Auto));
        }
    }
}
