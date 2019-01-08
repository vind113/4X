using Logic.PlayerClasses;

namespace Logic.SpaceObjects {
    public interface IPlanet {
        PlanetType Type { get; }

        void NextTurn(Player player);
    }
}