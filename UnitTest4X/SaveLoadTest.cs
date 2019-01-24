using Logic.GameClasses;
using NUnit.Framework;
using _4XGame.Serialization;
using System;
using System.ComponentModel;
using System.IO;

namespace UnitTest4X {
    [TestFixture]
    public class SaveLoadTest {
        [TestCase]
        public void SaveAndLoad_CorrectGame_SavedAndLoaded() {
            Game game = new Game();

            for (int i = 0; i < 1200; i++) {
                game.NextTurn();
            }

            string path = @"C:\Users\Tom\Desktop\test save.dat";

            GameSaveLoad.Save(game, path);

            Game loadedGame = GameSaveLoad.Load(path);

            Assert.AreEqual(game.Player.OwnedPlanets, loadedGame.Player.OwnedPlanets);
        }

        [TestCase]
        public void SaveAndLoad_PathIncorrect_ExceptionThrown() {
            Assert.Throws<SaveFileException>(LoadGameWithIncorrectPath);
        }

        private static void LoadGameWithIncorrectPath() {
            string path = @"C:\test.dat";

            Game loadedGame = GameSaveLoad.Load(path);
        }
    }
}
