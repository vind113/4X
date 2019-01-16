namespace Logic.Buildings {
    /// <summary>
    /// Представляет обитаемый космический объект - мир-кольцо.
    /// </summary>
    public class RingWorld : SpaceBuilding {
        private readonly static byte quality = 150;

        public RingWorld(string name) : base(name, 0, 100) {

        }

        public static byte Quality { get => quality; }
    }
}
