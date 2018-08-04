using Logic.GameClasses;
using Logic.PlayerClasses;
using Logic.SpaceObjects;
using NUnit.Framework;
using System;

namespace UnitTest4X {
    [TestFixture]
    public class PlanetTest {
        [TestCase]
        public void ExtractResourses_CorrectConditions_ResoursesMined() {

            double hydrogenPre = 0;
            double commonMetalsPre = 0;
            double rareElementsPre = 0;

            Player player = new Player();
            var system = player.StarSystems[0];

            foreach (var planet in system.SystemPlanets) {
                hydrogenPre += planet.BodyResourse.Hydrogen;
                commonMetalsPre += planet.BodyResourse.CommonMetals;
                rareElementsPre += planet.BodyResourse.RareEarthElements;
            }

            hydrogenPre += player.OwnedResourses.Hydrogen;
            commonMetalsPre += player.OwnedResourses.CommonMetals;
            rareElementsPre += player.OwnedResourses.RareEarthElements;

            const int turns = 10000;
            for (int i = 0; i < turns; i++) {
                system.NextTurn(player);
            }

            double hydrogenPost = player.OwnedResourses.Hydrogen;
            double commonMetalsPost = player.OwnedResourses.CommonMetals;
            double rareElementsPost = player.OwnedResourses.RareEarthElements;

            foreach (var planet in system.SystemPlanets) {
                hydrogenPost += planet.BodyResourse.Hydrogen;
                commonMetalsPost += planet.BodyResourse.CommonMetals;
                rareElementsPost += planet.BodyResourse.RareEarthElements;
            }

            double difference = 10_000_000;

            bool hydrogenEqual = CompareDoubles(hydrogenPre, hydrogenPost, difference);
            bool commonMetalsEqual = CompareDoubles(commonMetalsPre, commonMetalsPost, difference);
            bool rareElementsEqual = CompareDoubles(rareElementsPre, rareElementsPost, difference);

            /*
            Assert.Warn($"{Environment.NewLine}" +
                $"{hydrogenPre} and {hydrogenPost} ({hydrogenPre - hydrogenPost}) {hydrogenEqual} {Environment.NewLine}" +
                $"{commonMetalsPre} and {commonMetalsPost} ({(commonMetalsPre - commonMetalsPost)}) {commonMetalsEqual} {Environment.NewLine}" +
                $"{rareElementsPre} and {rareElementsPost} ({(rareElementsPre - rareElementsPost)}) {rareElementsEqual} {Environment.NewLine}");
            */

            Assert.IsTrue(hydrogenEqual && commonMetalsEqual && rareElementsEqual);
        }
      
        private static bool CompareDoubles(double numberOne, double numberTwo, double difference) {
            return Math.Abs(numberOne - numberTwo) <= difference;
        }
    }
}
