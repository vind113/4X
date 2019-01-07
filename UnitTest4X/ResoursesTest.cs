using Logic.Resource;
using NUnit.Framework;
using System;

namespace UnitTest4X {
    [TestFixture]
    public class ResourcesTest {
        [TestCase]
        public void Add_NoOverFlow_CorrectResult() {
            double hydrogenA = 1E10;
            double hydrogenB = 1E10;

            double commonMetalsA = 1E10;
            double commonMetalsB = 1E10;

            double rareElementsA = 1E10;
            double rareElementsB = 1E10;

            Resources a = new Resources(hydrogenA, commonMetalsA, rareElementsA);
            Resources b = new Resources(hydrogenB, commonMetalsB, rareElementsB);

            a.Add(b);

            bool hydrogenEqual = a.Hydrogen == (hydrogenA + hydrogenB);
            bool commonMetalsEqual = a.CommonMetals == (commonMetalsA + commonMetalsB);
            bool rareElementsEqual = a.RareEarthElements == (rareElementsA + rareElementsB);

            Assert.IsTrue(hydrogenEqual && commonMetalsEqual && rareElementsEqual);
        }

        [TestCase]
        public void Add_OverflowCaused_ExeptionThrown() {
            Assert.Throws(typeof(ArgumentException), new TestDelegate(AddWithOverflow));
        }

        private static void AddWithOverflow() {
            double hydrogenA = 1E308;
            double hydrogenB = 1E308;

            double commonMetalsA = 1E30;
            double commonMetalsB = 1E11;

            double rareElementsA = 1E10;
            double rareElementsB = 1E11;

            Resources a = new Resources(hydrogenA, commonMetalsA, rareElementsA);
            Resources b = new Resources(hydrogenB, commonMetalsB, rareElementsB);

            a.Add(b);
            Resources c = a;
        }

        [TestCase]
        public void Subtract_NoOverFlow_CorrectResult() {
            double hydrogenA = 1E11;
            double hydrogenB = 1E10;

            double commonMetalsA = 1E11;
            double commonMetalsB = 1E10;

            double rareElementsA = 1E11;
            double rareElementsB = 1E10;

            Resources a = new Resources(hydrogenA, commonMetalsA, rareElementsA);
            Resources b = new Resources(hydrogenB, commonMetalsB, rareElementsB);

            a.Subtract(b);
            Resources c = a;

            bool hydrogenEqual = c.Hydrogen == (hydrogenA - hydrogenB);
            bool commonMetalsEqual = c.CommonMetals == (commonMetalsA - commonMetalsB);
            bool rareElementsEqual = c.RareEarthElements == (rareElementsA - rareElementsB);

            Assert.IsTrue(hydrogenEqual && commonMetalsEqual && rareElementsEqual);
        }

        [TestCase]
        public void Subtract_SecondArgumentGreater_ExeptionThrown() {
            Assert.Throws(typeof(ArgumentException), new TestDelegate(SubtractWithException));
        }

        private static void SubtractWithException() {
            double hydrogenA = 1E10;
            double hydrogenB = 1E11;

            double commonMetalsA = 1E10;
            double commonMetalsB = 1E11;

            double rareElementsA = 1E10;
            double rareElementsB = 1E11;

            Resources a = new Resources(hydrogenA, commonMetalsA, rareElementsA);
            Resources b = new Resources(hydrogenB, commonMetalsB, rareElementsB);

            a.Subtract(b);
            Resources c = a;
        }

        [TestCase]
        public void Multiply_CorrectConditions_ResourcesMultiplied() {
            double resourceAmount = 100_000_000;
            double multiplier = 34;
            double expected = resourceAmount * multiplier;

            Resources resourceToMultiply = new Resources(resourceAmount, resourceAmount, resourceAmount);
            resourceToMultiply.Multiply(multiplier);

            Assert.AreEqual(expected, resourceToMultiply.Hydrogen, 0.99);
            Assert.AreEqual(expected, resourceToMultiply.CommonMetals, 0.99);
            Assert.AreEqual(expected, resourceToMultiply.RareEarthElements, 0.99);
        }
    }
}
