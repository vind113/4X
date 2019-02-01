using Logic.Resource;
using Logic.TechnologyClasses;
using Moq;
using NUnit.Framework;
using System;

namespace UnitTest4X {
    [TestFixture]
    public class TechnologyResearchTest {
        [TestCase]
        public void OneTurnProgress_ResourcesObjectIsNull_ExceptionThrown() {
            var research =
                new TechnologyResearch(new TechnologyChoice(), new ReadOnlyResources(), 10);

            Assert.Throws<ArgumentNullException>(() => research.OneTurnProgress(null));
        }

        [TestCase]
        public void OneTurnProgress_NotEnoughResources_NoProgressMade() {
            var resourcesMock = new Mock<IMutableResources>();
            resourcesMock.Setup(x => x.CanSubtract(It.IsNotNull<IBasicResources>())).Returns(false);

            var research =
                new TechnologyResearch(new TechnologyChoice(), new ReadOnlyResources(), 10);

            research.OneTurnProgress(resourcesMock.Object);
            research.OneTurnProgress(resourcesMock.Object);

            Assert.AreEqual(0, research.ResearchProgress);
        }

        [TestCase]
        public void OneTurnProgress_EnoughResources_ProgressMade() {
            var resourcesMock = new Mock<IMutableResources>();
            resourcesMock.Setup(x => x.CanSubtract(It.IsNotNull<IBasicResources>())).Returns(true);
            resourcesMock.Setup(x => x.Subtract(It.IsNotNull<IBasicResources>()));

            var research =
                new TechnologyResearch(new TechnologyChoice(), new ReadOnlyResources(), 10);

            research.OneTurnProgress(resourcesMock.Object);
            research.OneTurnProgress(resourcesMock.Object);

            Assert.AreEqual(2, research.ResearchProgress);

            research.OneTurnProgress(resourcesMock.Object);
            research.OneTurnProgress(resourcesMock.Object);

            Assert.AreEqual(4, research.ResearchProgress);
        }

        [TestCase]
        public void OneTurnProgress_ResearchDone_EventRaised() {
            var resourcesMock = new Mock<IMutableResources>();
            resourcesMock.Setup(x => x.CanSubtract(It.IsNotNull<IBasicResources>())).Returns(true);
            resourcesMock.Setup(x => x.Subtract(It.IsNotNull<IBasicResources>()));

            const int researchDuration = 10;
            var research =
                new TechnologyResearch(new TechnologyChoice(), new ReadOnlyResources(), researchDuration);

            bool eventRaised = false;
            research.ResearchCompleted += (sender, args) => eventRaised = true;

            Assert.AreEqual(false, eventRaised);

            for (int i = 0; i < researchDuration; i++) {
                research.OneTurnProgress(resourcesMock.Object);
            }

            Assert.AreEqual(true, eventRaised);
        }
    }
}
