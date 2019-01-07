using Logic.Resourse;
using NUnit.Framework;
using System;

namespace UnitTest4X {
    [TestFixture]
    public class ResoursesTest {
        [TestCase]
        public void Add_NoOverFlow_CorrectResult() {
            double hydrogenA = 1E10;
            double hydrogenB = 1E10;

            double commonMetalsA = 1E10;
            double commonMetalsB = 1E10;

            double rareElementsA = 1E10;
            double rareElementsB = 1E10;

            Resourses a = new Resourses(hydrogenA, commonMetalsA, rareElementsA);
            Resourses b = new Resourses(hydrogenB, commonMetalsB, rareElementsB);

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

            Resourses a = new Resourses(hydrogenA, commonMetalsA, rareElementsA);
            Resourses b = new Resourses(hydrogenB, commonMetalsB, rareElementsB);

            a.Add(b);
            Resourses c = a;
        }

        [TestCase]
        public void Subtract_NoOverFlow_CorrectResult() {
            double hydrogenA = 1E11;
            double hydrogenB = 1E10;

            double commonMetalsA = 1E11;
            double commonMetalsB = 1E10;

            double rareElementsA = 1E11;
            double rareElementsB = 1E10;

            Resourses a = new Resourses(hydrogenA, commonMetalsA, rareElementsA);
            Resourses b = new Resourses(hydrogenB, commonMetalsB, rareElementsB);

            a.Substract(b);
            Resourses c = a;

            bool hydrogenEqual = c.Hydrogen == (hydrogenA - hydrogenB);
            bool commonMetalsEqual = c.CommonMetals == (commonMetalsA - commonMetalsB);
            bool rareElementsEqual = c.RareEarthElements == (rareElementsA - rareElementsB);

            Assert.IsTrue(hydrogenEqual && commonMetalsEqual && rareElementsEqual);
        }

        [TestCase]
        public void Subtract_SecondArgumentGreater_ExeptionThrown() {
            Assert.Throws(typeof(ArgumentException), new TestDelegate(SubstractWithException));
        }

        private static void SubstractWithException() {
            double hydrogenA = 1E10;
            double hydrogenB = 1E11;

            double commonMetalsA = 1E10;
            double commonMetalsB = 1E11;

            double rareElementsA = 1E10;
            double rareElementsB = 1E11;

            Resourses a = new Resourses(hydrogenA, commonMetalsA, rareElementsA);
            Resourses b = new Resourses(hydrogenB, commonMetalsB, rareElementsB);

            a.Substract(b);
            Resourses c = a;
        }

        [TestCase]
        public void Multiply_CorrectConditions_ResoursesMultiplied() {
            double resourseAmount = 100_000_000;
            double multiplier = 34;
            double expected = resourseAmount * multiplier;

            Resourses resourseToMultiply = new Resourses(resourseAmount, resourseAmount, resourseAmount);
            resourseToMultiply.Multiply(multiplier);

            Assert.AreEqual(expected, resourseToMultiply.Hydrogen, 0.99);
            Assert.AreEqual(expected, resourseToMultiply.CommonMetals, 0.99);
            Assert.AreEqual(expected, resourseToMultiply.RareEarthElements, 0.99);
        }

        /*[TestCase]
        public void Multiply_Overflow_ExceptionThrown() {
            double resourseAmount = 1E308;
            double multiplier = 340;

            Resourses resourseToMultiply = new Resourses(resourseAmount, resourseAmount, resourseAmount);

            Assert.Throws<OverflowException>(() => {
                resourseToMultiply.Multiply(multiplier);
                resourseToMultiply.Substract(new Resourses(resourseAmount, resourseAmount, resourseAmount));
            });
        }*/

    }
}
