using Logic.PlayerClasses;
using Logic.Resource;
using NUnit.Framework;
using System;

namespace UnitTest4X {
    [TestFixture]
    public class ShipsTest {
        [TestCase]
        public void ColonizerGetColonists_ParamIsLowerThanColonistsOnShip_GetColonists() {
            Ships ships = new Ships();

            Colonizer colonizer = ships.GetColonizer(new Resources(double.MaxValue, double.MaxValue, double.MaxValue));

            double colonize = colonizer.GetColonists(colonizer.ColonistsOnShip / 4);

            Assert.True(colonize == (Colonizer.Colonists / 4));
        }

        [TestCase]
        public void ColonizerGetColonists_ParamIsGreaterThanColonistsOnShip_GetColonists() {
            Ships ships = new Ships();

            Colonizer colonizer = ships.GetColonizer(new Resources(double.MaxValue, double.MaxValue, double.MaxValue));

            double colonize = colonizer.GetColonists(colonizer.ColonistsOnShip * 4);

            Assert.Zero(colonize);
        }

        [TestCase]
        public void MinerGetMiners_CannotAffordMiners_ReturnZero() {
            int shipsToBuy = 10;
            int resourcesInitModifier = 2;

            Resources minerPrice = Miner.Price;

            Resources inPossesion = new Resources(
                resourcesInitModifier*minerPrice.Hydrogen,
                resourcesInitModifier*minerPrice.CommonMetals,
                resourcesInitModifier*minerPrice.RareEarthElements
            );

            Resources neededResources = new Resources(
                shipsToBuy * minerPrice.Hydrogen,
                shipsToBuy * minerPrice.CommonMetals,
                shipsToBuy * minerPrice.RareEarthElements
            );

            int result = 0;
            Ships ships = new Ships();

            result = ships.GetMiners(inPossesion, shipsToBuy);

            Assert.AreEqual(0, result);
        }

        [TestCase]
        public void MinerGetMiners_CanAffordMiners_ReturnMinersQuantity() {
            int shipsToBuy = 10;
            int resourcesInitModifier = 25;

            Resources minerPrice = Miner.Price;

            Resources inPossesion = new Resources(
                resourcesInitModifier * minerPrice.Hydrogen,
                resourcesInitModifier * minerPrice.CommonMetals,
                resourcesInitModifier * minerPrice.RareEarthElements
            );

            Resources neededResources = new Resources(
                shipsToBuy * minerPrice.Hydrogen,
                shipsToBuy * minerPrice.CommonMetals,
                shipsToBuy * minerPrice.RareEarthElements
            );

            int result = 0;
            Ships ships = new Ships();

            result = ships.GetMiners(inPossesion, shipsToBuy);

            Assert.AreEqual(shipsToBuy, result);
        }

        [TestCase]
        public void MinerMine_FromIsZero_ImmediateReturn() {
            Resources from = new Resources(0, 0, 0);

            Resources to = new Resources(1E5, 1E5, 1E5);

            int miners = 10;

            Miner.Mine(miners, from, to);

            Assert.IsTrue(Resources.AreEqual(from, Resources.Zero));
        }

        [TestCase]
        public void MinerMine_FromGreaterThanTo_CorrectExtraction() {
            double hydrogenFrom = 1E6;
            double commonMetalsFrom = 12E6;
            double rareElementsFrom = 23E6;

            double hydrogenTo = 1E5;
            double commonMetalsTo = 1E5;
            double rareElementsTo = 1E5;

            Resources from = new Resources(hydrogenFrom, commonMetalsFrom, rareElementsFrom);
            Resources to = new Resources(hydrogenTo, commonMetalsTo, rareElementsTo);

            Resources sumBefore = new Resources(
                from.Hydrogen + to.Hydrogen,
                from.CommonMetals + to.CommonMetals,
                from.RareEarthElements + to.RareEarthElements
            );

            int miners = 10;

            Miner.Mine(miners, from, to);

            Resources sumAfter = new Resources(
                from.Hydrogen + to.Hydrogen,
                from.CommonMetals + to.CommonMetals,
                from.RareEarthElements + to.RareEarthElements
            );

            bool isResourcesAmountSame = Resources.AreEqual(sumBefore, sumAfter);

            bool isFromResourcesDecreased = (hydrogenFrom > from.Hydrogen)
                                         && (commonMetalsFrom > from.CommonMetals)
                                         && (rareElementsFrom > from.RareEarthElements);

            bool isToResourcesIncreased = (hydrogenTo < to.Hydrogen)
                                        && (commonMetalsTo < to.CommonMetals)
                                        && (rareElementsTo < to.RareEarthElements);

            Assert.IsTrue(isResourcesAmountSame && isFromResourcesDecreased && isToResourcesIncreased);
        }

        [TestCase]
        public void MinerMine_FromLowerThanTo_FromIsZero() {
            double hydrogenFrom = 1E4;
            double commonMetalsFrom = 1E3;
            double rareElementsFrom = 2E3;

            double hydrogenTo = 1E5;
            double commonMetalsTo = 1E5;
            double rareElementsTo = 1E5;

            Resources from = new Resources(hydrogenFrom, commonMetalsFrom, rareElementsFrom);
            Resources to = new Resources(hydrogenTo, commonMetalsTo, rareElementsTo);

            int miners = 10;
            Miner.Mine(miners, from, to);

            bool isFromIsZero = Resources.AreEqual(from, Resources.Zero);

            bool isToResourcesIncreased = (hydrogenTo < to.Hydrogen)
                                        && (commonMetalsTo < to.CommonMetals)
                                        && (rareElementsTo < to.RareEarthElements);

            Assert.IsTrue(isFromIsZero && isToResourcesIncreased);
        }

    }
}
