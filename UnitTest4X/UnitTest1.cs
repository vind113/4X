using System;
using NUnit.Framework;

using Logic.GameClasses;
using Logic.Resourse;

namespace UnitTest4X {
    [TestFixture]
    public class UnitTest1 {
        #region Next Turn
        [TestCase]
        public void NextTurn_PassTurn_DateIsCorrect() {
            Game.ResetDate();
            int months = 120000;
            for (int index = 0; index < months; index++) {
                Game.NextTurn();
            }
            Assert.AreEqual("1.12500", Game.GameDate);
        }

        [TestCase]
        public void NextTurn_PassTurn_TurnIsCorrect() {
            Game.ResetDate();
            int months = 120;
            for (int index = 1; index < months; index++) {
                Game.NextTurn();
            }
            Assert.AreEqual(months, Game.GameTurn);
        }
        #endregion

        #region Citizen Hub People Property
        [TestCase]
        public void CitizenHubProperty_CorrectNumber_ValueIsSet() {
            double peopleCount = 11;
            Game.Player.PlayerCitizenHub.MaximumCount = peopleCount;

            Game.Player.PlayerCitizenHub.CitizensInHub = peopleCount;

            Assert.AreEqual(Game.Player.PlayerCitizenHub.CitizensInHub, peopleCount);
        }

        [TestCase]
        public void CitizenHubProperty_NumberIsLowerThanZero_ValueIsNotSet() {
            double peopleCount = -1;
            Game.Player.PlayerCitizenHub.CitizensInHub = peopleCount;

            Assert.AreNotEqual(Game.Player.PlayerCitizenHub.CitizensInHub, peopleCount);
        }

        [TestCase]
        public void CitizenHubProperty_NumberIsBiggerThanMaxCount_ValueIsNotSet() {
            double peopleCount = Game.Player.PlayerCitizenHub.MaximumCount + 1;
            Game.Player.PlayerCitizenHub.CitizensInHub = peopleCount;

            Assert.AreNotEqual(Game.Player.PlayerCitizenHub.CitizensInHub, peopleCount);
        }
        #endregion

        #region Citizen Hub Max Property
        [TestCase]
        public void CitizenHubMax_MaxCountCorrect_ValueIsSet() {
            double maxCount = 1;

            Game.Player.PlayerCitizenHub.MaximumCount = maxCount;

            Assert.AreEqual(Game.Player.PlayerCitizenHub.MaximumCount, maxCount);
        }

        [TestCase]
        public void CitizenHubMax_MaxCountLowerThanZero_ValueIsNotSet() {
            double maxCount = -1;

            Game.Player.PlayerCitizenHub.MaximumCount = maxCount;

            Assert.AreNotEqual(Game.Player.PlayerCitizenHub.MaximumCount, maxCount);
        }
        #endregion

        #region Player Resourse Property 

        [TestCase]
        public void PlayerResoursesProperty_ComMetalIsLowerThanZero_ValueIsNotSet() {
            Resourses resourses = new Resourses(1, -1, 1);

            Game.Player.PlayerResourses = resourses;

            Assert.AreNotEqual(Game.Player.PlayerResourses, resourses);
        }

        [TestCase]
        public void PlayerResoursesProperty_RareMetalIsLowerThanZero_ValueIsNotSet() {
            Resourses resourses = new Resourses(1, 1, -1);

            Game.Player.PlayerResourses = resourses;

            Assert.AreNotEqual(Game.Player.PlayerResourses, resourses);
        }

        [TestCase]
        public void PlayerResoursesProperty_HydorgenIsLowerThanZero_ValueIsNotSet() {
            Resourses resourses = new Resourses(-100, 1, 1);

            Game.Player.PlayerResourses = resourses;

            Assert.AreNotEqual(Game.Player.PlayerResourses, resourses);
        }

        [TestCase]
        public void PlayerResoursesProperty_ResoursesAreValid_ValueIsSet() {
            Resourses resourses = new Resourses(1, 1, 1);

            Game.Player.PlayerResourses = resourses;

            Assert.AreEqual(Game.Player.PlayerResourses, resourses);
        }

        #endregion
    }
}
