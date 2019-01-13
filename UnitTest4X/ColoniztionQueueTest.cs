using System;
using System.Collections.Generic;
using NUnit.Framework;
using Logic.Resource;
using Logic.SpaceObjects;
using Logic.PlayerClasses;
using Moq;

namespace UnitTest4X {
    [TestFixture]
    public class ColoniztionQueueTest {
        [TestCase]
        public void ColonizeWhilePossible_QueueCountIsZero_ImmediateReturn() {
            ColoniztionQueue coloniztionQueue = new ColoniztionQueue();

            Assert.DoesNotThrow(() => {
                coloniztionQueue.ColonizeWhilePossible(new Mock<IShips>().Object);
            });
        }

        [TestCase]
        public void ColonizeWhilePossible_QueueCountIsGreaterThanZero() {
            int planetsToColonize = 10;

            List<IHabitablePlanet> planetList = new List<IHabitablePlanet>();
            
            for (int i = 0; i < planetsToColonize; i++) {
                var mock = new Mock<IHabitablePlanet>();
                
                mock.Setup(x => x.Colonize(It.IsNotNull<Colonizer>()) )
                    .Returns(ColonizationState.Colonized);

                planetList.Add(mock.Object);
            }

            ColoniztionQueue coloniztionQueue = new ColoniztionQueue(planetList);

            var shipsMock = new Mock<IShips>();

            shipsMock.Setup(x => x.GetColonizer())
                .Returns(Colonizer.GetColonizer());

            var resourcesMock = new Mock<IResources>();

            coloniztionQueue.ColonizeWhilePossible(shipsMock.Object);

            Assert.AreEqual(0, coloniztionQueue.PlanetsInQueue);
        }
    }
}
