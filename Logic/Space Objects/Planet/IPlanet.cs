using Logic.PlayerClasses;

namespace Logic.SpaceObjects {
    public interface IPlanet {
        int AvailableSites { get; }
        int BuildingSites { get; }
        bool IsColonized { get; }
        PlanetType Type { get; }

        ColonizationState Colonize(Player player);
        void NextTurn(Player player);
    }
}