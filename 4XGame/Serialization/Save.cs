using Logic.GameClasses;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace _4XGame.Serialization {
    //TODO: Descriptive name
    public static class GameSaveLoad {
        public static void Save(Game game, string path) {
            if (game == null) {
                throw new System.ArgumentNullException(nameof(game));
            }

            if (path == null) {
                throw new System.ArgumentNullException(nameof(path));
            }

            using (Stream s = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write)) {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(s, game);
            }
        }

        public static Game Load(string path) {
            if (path == null) {
                throw new System.ArgumentNullException(nameof(path));
            }

            Game loadedGame = new Game();

            try {
                using (Stream s = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                    BinaryFormatter formatter = new BinaryFormatter();
                    loadedGame = (Game)formatter.Deserialize(s);
                }
            }
            catch (FileNotFoundException) {
                throw new SaveFileException("Cannot load game: Path incorrect");
            }
            catch (SerializationException) {
                throw new SaveFileException("Cannot load game: File format incorrect");
            }

            return loadedGame;
        }
    }
}
