using Logic.GameClasses;
using Logic.PlayerClasses;
using Logic.SpaceObjects;
using NUnit.Framework;
using System;

namespace UnitTest4X {
    [TestFixture]
    public class PlanetTest {
        [TestCase]
        public void ExtractResources_CorrectConditions_ResourcesMined() {

            double hydrogenPre = 0;
            double commonMetalsPre = 0;
            double rareElementsPre = 0;

            Player player = new Player();
            var planet = player.StarSystems[0].SystemPlanets[2];

            hydrogenPre += planet.BodyResource.Hydrogen;
            commonMetalsPre += planet.BodyResource.CommonMetals;
            rareElementsPre += planet.BodyResource.RareEarthElements;

            hydrogenPre += player.OwnedResources.Hydrogen;
            commonMetalsPre += player.OwnedResources.CommonMetals;
            rareElementsPre += player.OwnedResources.RareEarthElements;

            const int turns = 10000;
            for (int i = 0; i < turns; i++) {
                planet.NextTurn(player);
            }

            double hydrogenPost = player.OwnedResources.Hydrogen;
            double commonMetalsPost = player.OwnedResources.CommonMetals;
            double rareElementsPost = player.OwnedResources.RareEarthElements;

            hydrogenPost += planet.BodyResource.Hydrogen;
            commonMetalsPost += planet.BodyResource.CommonMetals;
            rareElementsPost += planet.BodyResource.RareEarthElements;
            
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
