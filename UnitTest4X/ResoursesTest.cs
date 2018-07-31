﻿using Logic.Resourse;
using NUnit.Framework;
using System;

namespace UnitTest4X {
    [TestFixture]
    public class ResoursesTest {
        [TestCase]
        public void Plus_NoOverFlow_CorrectResult() {
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
        public void Minus_NoOverFlow_CorrectResult() {
            double hydrogenA = 1E11;
            double hydrogenB = 1E10;

            double commonMetalsA = 1E11;
            double commonMetalsB = 1E10;

            double rareElementsA = 1E11;
            double rareElementsB = 1E10;

            Resourses a = new Resourses(hydrogenA, commonMetalsA, rareElementsA);
            Resourses b = new Resourses(hydrogenB, commonMetalsB, rareElementsB);

            Resourses c = a.Substract(b);

            bool hydrogenEqual = c.Hydrogen == (hydrogenA - hydrogenB);
            bool commonMetalsEqual = c.CommonMetals == (commonMetalsA - commonMetalsB);
            bool rareElementsEqual = c.RareEarthElements == (rareElementsA - rareElementsB);

            Assert.IsTrue(hydrogenEqual && commonMetalsEqual && rareElementsEqual);
        }

        [TestCase]
        public void Minus_SecondArgumentGreater_ExeptionThrown() {
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

            Resourses c = a.Substract(b);
        }
    }
}
