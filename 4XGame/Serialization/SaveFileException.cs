using System;

namespace _4XGame.Serialization {
    public class SaveFileException : Exception {
        public SaveFileException() {

        }

        public SaveFileException(string message) : base(message) {

        }
    }
}
