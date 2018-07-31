﻿using System;
using NUnit.Framework;
using Logic.GameClasses;
using Logic.Resourse;
using Logic.SpaceObjects;
using Logic.PlayerClasses;
using System.Collections.Generic;

namespace UnitTest4X {
    [TestFixture]
    public class UnitTest1 {
        [TestCase]
        public void CheckDateIncrement_CorrectConditions_DateIsCorrect() {
            TurnDate currentDate = new TurnDate();
            for (int i = 0; i < 120_000; i++) {
                currentDate = currentDate.NextTurn();
            }
            Assert.AreEqual(currentDate.Turn, 120_001);
            Assert.AreEqual(currentDate.Date, "1.12500");
        }

        #region Star Luminosity
        [TestCase]
        public void GetLuminosityClass_OClassPassed_StarIsOClass() {
            Star oStar = new Star("O star", 696_392d * 6.7, LuminosityClass.O);
            Assert.AreEqual(oStar.LumClass, LuminosityClass.O);
        }

        [TestCase]
        public void GetLuminosityClass_BClassPassed_StarIsBClass() {
            Star oStar = new Star("B star", 696_392d * 6.5, LuminosityClass.B);
            Assert.AreEqual(oStar.LumClass, LuminosityClass.B);
        }

        [TestCase]
        public void GetLuminosityClass_AClassPassed_StarIsAClass() {
            Star oStar = new Star("A star", 696_392d * 1.7, LuminosityClass.A);
            Assert.AreEqual(oStar.LumClass, LuminosityClass.A);
        }

        [TestCase]
        public void GetLuminosityClass_FClassPassed_StarIsFClass() {
            Star oStar = new Star("F star", 696_392d * 1.3, LuminosityClass.F);
            Assert.AreEqual(oStar.LumClass, LuminosityClass.F);
        }

        [TestCase]
        public void GetLuminosityClass_GClassPassed_StarIsGClass() {
            Star oStar = new Star("G star", 696_392d * 1.14, LuminosityClass.G);
            Assert.AreEqual(oStar.LumClass, LuminosityClass.G);
        }

        [TestCase]
        public void GetLuminosityClass_KClassPassed_StarIsKClass() {
            Star oStar = new Star("K star", 696_392d * 0.95, LuminosityClass.K);
            Assert.AreEqual(oStar.LumClass, LuminosityClass.K);
        }

        [TestCase]
        public void GetLuminosityClass_MClassPassed_StarIsMClass() {
            Star oStar = new Star("M star", 696_392d * 0.699, LuminosityClass.M);
            Assert.AreEqual(oStar.LumClass, LuminosityClass.M);
        }
        #endregion

        [TestCase]
        public void TryToColonizeQueue_QueueCountIsZero_ImmediateReturn() {
            Player player = new Player();

            Assert.DoesNotThrow(new TestDelegate(player.TryToColonizeQueue));
        }

        [TestCase]
        public void TryToColonizeQueue_QueueCountIsGreaterThanZero() {
            Player player = new Player();
            int planetsToColonize = 255;

            player.OwnedResourses = new Resourses(Double.MaxValue, Double.MaxValue, Double.MaxValue);

            List<Planet> planetList = new List<Planet>();

            for (int i = 0; i < planetsToColonize; i++) {
                planetList.Add(PlanetFactory.GetPlanet("a", PlanetTypeValue.Continental));
            }

            player.AddStarSystem(new StarSystem("system", new List<Star>(), planetList));

            foreach (var system in player.StarSystems) {
                for (int planetIndex = 0; planetIndex < system.PlanetsCount; planetIndex++) {
                    player.AddToColonizationQueue(system.SystemPlanets[planetIndex]);
                }
            }

            player.TryToColonizeQueue();

            Assert.AreEqual(planetsToColonize + 2, player.ColonizedPlanets);
        }
    }
}
