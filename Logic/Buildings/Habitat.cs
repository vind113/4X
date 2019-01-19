namespace Logic.Buildings {
    /// <summary>
    /// Представляет обитаемую космическую станцию
    /// </summary>
    public class Habitat : SpaceBuilding {
        public static byte Quality { get; } = 100;

        public Habitat(string name) : base(name, 0, 20_000_000_000) {

        }
    }
}
