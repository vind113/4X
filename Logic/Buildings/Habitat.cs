namespace Logic.Buildings {
    /// <summary>
    /// Представляет обитаемую космическую станцию
    /// </summary>
    public class Habitat : SpaceBuilding {
        private readonly static short buildingTime = 24;
        private readonly static byte quality = 100;

        private string name;

        public Habitat(string name) : base(0, 20_000_000_000) {
            this.name = name;
        }

        public static short BuildingTime { get => buildingTime; }
        public static byte Quality { get => quality; }
        public string Name { get => this.name; }
    }
}
