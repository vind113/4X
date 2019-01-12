using Logic.PlayerClasses;

namespace Logic.SpaceObjects {
    public interface IHabitablePlanet : IHabitable {
        int AvailableSites { get; }
        int BuildingSites { get; }
        bool IsColonized { get; }

        ColonizationState Colonize(Colonizer colonizer);
    }
}
