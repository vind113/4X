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

            foreach (var planet in player.StarSystems[0].SystemPlanets) {
                hydrogenPre += planet.BodyResourse.Hydrogen;
                commonMetalsPre += planet.BodyResourse.CommonMetals;
                rareElementsPre += planet.BodyResourse.RareEarthElements;
            }

            const int turns = 10000;
            for (int i = 0; i < turns; i++) {
                player.NextTurn(false, false);
            }

            double hydrogenPost = player.OwnedResourses.Hydrogen;
            double commonMetalsPost = player.OwnedResourses.CommonMetals;
            double rareElementsPost = player.OwnedResourses.RareEarthElements;

            foreach (var planet in player.StarSystems[0].SystemPlanets) {
                hydrogenPost += planet.BodyResourse.Hydrogen;
                commonMetalsPost += planet.BodyResourse.CommonMetals;
                rareElementsPost += planet.BodyResourse.RareEarthElements;
            }

            double difference = 10_000_000;

            bool hydrogenEqual = CompareDoubles(hydrogenPre, hydrogenPost, difference);
            bool commonMetalsEqual = CompareDoubles(commonMetalsPre, commonMetalsPost, difference);
            bool rareElementsEqual = CompareDoubles(rareElementsPre, rareElementsPost, difference);

            Assert.IsTrue(hydrogenEqual && commonMetalsEqual && rareElementsEqual,
                $"{Environment.NewLine}" +
                $"{hydrogenPre} and {hydrogenPost} {hydrogenEqual} {Environment.NewLine}" +
                $"{commonMetalsPre} and {commonMetalsPost} {commonMetalsEqual} {Environment.NewLine}" +
                $"{rareElementsPre} and {rareElementsPost} {rareElementsEqual} {Environment.NewLine}");
        }
      
        private static bool CompareDoubles(double numberOne, double numberTwo, double difference) {
            return Math.Abs(numberOne - numberTwo) <= difference;
        }
    }
}
