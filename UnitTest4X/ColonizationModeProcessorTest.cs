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
        public void GetAutoColoniztionState_ModeIsNone_ReturnsFalse() {
            var playerMock = new Mock<IPlayer>();
            var modeDeifner = new ColonizationModeProcessor(playerMock.Object);
            Assert.IsFalse(modeDeifner.GetAutoColoniztionState(ColonizatonModes.None));
        }

        [TestCase]
        public void GetAutoColoniztionState_ModeIsAll_ReturnsTrue() {
            var playerMock = new Mock<IPlayer>();
            var modeDeifner = new ColonizationModeProcessor(playerMock.Object);
            Assert.IsTrue(modeDeifner.GetAutoColoniztionState(ColonizatonModes.All));
        }

        [TestCase]
        public void GetAutoColoniztionState_ModeIsAutoAndDensityOK_ReturnsTrue() {
            var playerMock = new Mock<IPlayer>();
            playerMock.SetupGet(x => x.Population).Returns(6_000_000_000);
            playerMock.SetupGet(x => x.ColonizedPlanets).Returns(1);

            var modeDefiner = new ColonizationModeProcessor(playerMock.Object);

            Assert.IsTrue(modeDefiner.GetAutoColoniztionState(ColonizatonModes.Auto));
        }

        [TestCase]
        public void GetAutoColoniztionState_ModeIsAutoAndDensityNotOk_ReturnsFalse() {
            var playerMock = new Mock<IPlayer>();
            playerMock.SetupGet(x => x.Population).Returns(9_000_000_000);
            playerMock.SetupGet(x => x.ColonizedPlanets).Returns(2);

            var modeDefiner = new ColonizationModeProcessor(playerMock.Object);

            Assert.IsFalse(modeDefiner.GetAutoColoniztionState(ColonizatonModes.Auto));
        }
    }
}
