using Logic.GameClasses;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace _4XGame.Serialization {
    public static class SavedGame {
        public static void Save(Game game, string path) {
            using(Stream s = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write)) {
                BinaryFormatter formatter = new BinaryFormatter();

                formatter.Serialize(s, game);
            }
        }

        public static Game Load(string path) {
            Game loadedGame = null;

            using (Stream s = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                BinaryFormatter formatter = new BinaryFormatter();

                loadedGame = (Game)formatter.Deserialize(s);
            }

            return loadedGame;
        }
    }
}
