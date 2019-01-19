namespace Logic.Buildings {
    /// <summary>
    /// Представляет обитаемый космический объект - мир-кольцо.
    /// </summary>
    public class RingWorld : SpaceBuilding {
        public static byte Quality { get; } = 150;

        public RingWorld(string name) : base(name, 0, 100) {

        }
    }
}
