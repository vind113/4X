using Logic.Resource;
using Logic.TechnologyClasses;
using Moq;
using NUnit.Framework;
using System;

namespace UnitTest4X {
    [TestFixture]
    public class ResearchQueueTest {
        [TestCase]
        public void Add_ResearchIsNull_ExceptionThrown() {
            Assert.Throws<ArgumentNullException>(() => new ResearchQueue().Add(null));
        }

        [TestCase]
        public void Add_ResearchAddedMoreThanOnce_OnlyAddedOnce() {
            var queue = new ResearchQueue();

            var research = new TechnologyResearcher(new EmptyTechnology(), new Resources(), 1);

            Assert.AreEqual(0, queue.BeingResearched);
            Assert.AreEqual(0, queue.WaitingInQueue);

            queue.Add(research);

            Assert.AreEqual(1, queue.BeingResearched);
            Assert.AreEqual(0, queue.WaitingInQueue);
        }

        [TestCase]
        public void Add_FourResearchesAdded_CorrectDisrtibution() {
            var queue = new ResearchQueue();

            var researchOne = new TechnologyResearcher(new EmptyTechnology(), new Resources(), 1);
            var researchTwo = new TechnologyResearcher(new EmptyTechnology(), new Resources(), 1);
            var researchThree = new TechnologyResearcher(new EmptyTechnology(), new Resources(), 1);
            var researchFour = new TechnologyResearcher(new EmptyTechnology(), new Resources(), 1);

            Assert.AreEqual(0, queue.BeingResearched);
            Assert.AreEqual(0, queue.WaitingInQueue);

            queue.Add(researchOne);

            Assert.AreEqual(1, queue.BeingResearched);
            Assert.AreEqual(0, queue.WaitingInQueue);

            queue.Add(researchTwo);

            Assert.AreEqual(2, queue.BeingResearched);
            Assert.AreEqual(0, queue.WaitingInQueue);

            queue.Add(researchThree);

            Assert.AreEqual(3, queue.BeingResearched);
            Assert.AreEqual(0, queue.WaitingInQueue);

            queue.Add(researchFour);

            Assert.AreEqual(3, queue.BeingResearched);
            Assert.AreEqual(1, queue.WaitingInQueue);
        }
    }
}
