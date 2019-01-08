using Logic.PlayerClasses;

namespace Logic.SpaceObjects {
    interface IHabitablePlanet : IHabitable {
        int AvailableSites { get; }
        int BuildingSites { get; }
        bool IsColonized { get; }

        ColonizationState Colonize(Player player);
    }
}
