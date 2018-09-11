using Logic.PopulationClasses;

namespace Logic.SpaceObjects {
    public interface IHabitable {
        long PopulationValue { get; }
        long MaximumPopulation { get; }
        Population Population { get; }
    }
}
