﻿using Logic.GameClasses;
using NUnit.Framework;
using _4XGame.Serialization;
using System;
using System.ComponentModel;

namespace UnitTest4X {
    [TestFixture]
    public class SaveLoadTest {
        [TestCase]
        public void SaveAndLoad_CorrectGame_SavedAndLoaded() {
            Game game = new Game();

            game.IsAutoColonizationEnabled = true;

            for (int i = 0; i < 1200; i++) {
                game.NextTurn();
            }

            string path = @"C:\Users\Tom\Desktop\test save.dat";

            SavedGame.Save(game, path);

            Game loadedGame = SavedGame.Load(path);

            Assert.AreEqual(game.Player.OwnedPlanets, loadedGame.Player.OwnedPlanets);
        }
    }
}
