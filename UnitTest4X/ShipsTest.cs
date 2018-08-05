using Logic.PlayerClasses;
using Logic.Resourse;
using NUnit.Framework;
using System;

namespace UnitTest4X {
    [TestFixture]
    public class ShipsTest {
        [TestCase]
        public void ColonizerGetColonists_ParamIsLowerThanColonistsOnShip_GetColonists() {
            Colonizer colonizer = Colonizer.TryGetColonizer(new Resourses(Double.MaxValue, Double.MaxValue, Double.MaxValue));

            double colonize = colonizer.GetColonists(colonizer.ColonistsOnShip / 4);

            Assert.True(colonize == (Colonizer.Colonists / 4));
        }

        [TestCase]
        public void ColonizerGetColonists_ParamIsGreaterThanColonistsOnShip_GetColonists() {
            Colonizer colonizer = Colonizer.TryGetColonizer(new Resourses(Double.MaxValue, Double.MaxValue, Double.MaxValue));

            double colonize = colonizer.GetColonists(colonizer.ColonistsOnShip * 4);

            Assert.Zero(colonize);
        }

        [TestCase]
        public void MinerGetMiners_CannotAffordMiners_ReturnZero() {
            int shipsToBuy = 10;
            int resoursesInitModifier = 2;

            Resourses minerPrice = Miner.Price;

            Resourses inPossesion = new Resourses(
                resoursesInitModifier*minerPrice.Hydrogen,
                resoursesInitModifier*minerPrice.CommonMetals,
                resoursesInitModifier*minerPrice.RareEarthElements
            );

            int result = Miner.TryGetMiners(shipsToBuy, inPossesion);

            Assert.AreEqual(0, result);
        }

        [TestCase]
        public void MinerGetMiners_CanAffordMiners_ReturnMinersQuantity() {
            int shipsToBuy = 10;
            int resoursesInitModifier = 25;

            Resourses minerPrice = Miner.Price;

            Resourses inPossesion = new Resourses(
                resoursesInitModifier * minerPrice.Hydrogen,
                resoursesInitModifier * minerPrice.CommonMetals,
                resoursesInitModifier * minerPrice.RareEarthElements
            );

            int result = Miner.TryGetMiners(shipsToBuy, inPossesion);

            Assert.AreEqual(shipsToBuy, result);
        }

        [TestCase]
        public void MinerMine_FromIsZero_ImmediateReturn() {
            Resourses from = new Resourses(0, 0, 0);

            Resourses to = new Resourses(1E5, 1E5, 1E5);

            int miners = 10;

            Miner.Mine(miners, from, to);

            Assert.IsTrue(Resourses.AreEqual(from, Resourses.Zero));
        }

        [TestCase]
        public void MinerMine_FromGreaterThanTo_CorrectExtraction() {
            double hydrogenFrom = 1E6;
            double commonMetalsFrom = 12E6;
            double rareElementsFrom = 23E6;

            double hydrogenTo = 1E5;
            double commonMetalsTo = 1E5;
            double rareElementsTo = 1E5;

            Resourses from = new Resourses(hydrogenFrom, commonMetalsFrom, rareElementsFrom);
            Resourses to = new Resourses(hydrogenTo, commonMetalsTo, rareElementsTo);

            Resourses sumBefore = new Resourses(
                from.Hydrogen + to.Hydrogen,
                from.CommonMetals + to.CommonMetals,
                from.RareEarthElements + to.RareEarthElements
            );

            int miners = 10;

            Miner.Mine(miners, from, to);

            Resourses sumAfter = new Resourses(
                from.Hydrogen + to.Hydrogen,
                from.CommonMetals + to.CommonMetals,
                from.RareEarthElements + to.RareEarthElements
            );

            bool isResoursesAmountSame = Resourses.AreEqual(sumBefore, sumAfter);

            bool isFromResoursesDecreased = (hydrogenFrom > from.Hydrogen)
                                         && (commonMetalsFrom > from.CommonMetals)
                                         && (rareElementsFrom > from.RareEarthElements);

            bool isToResoursesIncreased = (hydrogenTo < to.Hydrogen)
                                        && (commonMetalsTo < to.CommonMetals)
                                        && (rareElementsTo < to.RareEarthElements);

            Assert.IsTrue(isResoursesAmountSame && isFromResoursesDecreased && isToResoursesIncreased);
        }

        [TestCase]
        public void MinerMine_FromLowerThanTo_FromIsZero() {
            double hydrogenFrom = 1E4;
            double commonMetalsFrom = 1E3;
            double rareElementsFrom = 2E3;

            double hydrogenTo = 1E5;
            double commonMetalsTo = 1E5;
            double rareElementsTo = 1E5;

            Resourses from = new Resourses(hydrogenFrom, commonMetalsFrom, rareElementsFrom);
            Resourses to = new Resourses(hydrogenTo, commonMetalsTo, rareElementsTo);

            int miners = 10;
            Miner.Mine(miners, from, to);

            bool isFromZero = Resourses.AreEqual(from, Resourses.Zero);

            bool isToResoursesIncreased = (hydrogenTo < to.Hydrogen)
                                        && (commonMetalsTo < to.CommonMetals)
                                        && (rareElementsTo < to.RareEarthElements);

            Assert.IsTrue(isFromZero && isToResoursesIncreased);
        }

    }
}
