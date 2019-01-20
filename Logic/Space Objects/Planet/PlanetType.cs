using Logic.SpaceObjects.PlanetClasses;
using System;

namespace Logic.SpaceObjects {
    [Serializable]
    public struct PlanetType {
        public const double GoodWorldQuality = 100;

        public string Name { get; }
        public double Quality { get; }

        public double MiningDifficulty { get; }
        public byte ResourceAbundance { get; }

        public TemperatureClass TemperatureClass { get; }
        public VolatilesClass VolatilesClass { get; }
        public SubstancesClass SubstancesClass { get; }

        public PlanetType(TemperatureClass temperature, VolatilesClass volatiles, SubstancesClass substances) {
            this.TemperatureClass = temperature;
            this.VolatilesClass = volatiles;
            this.SubstancesClass = substances;

            double tempQuality = PlanetCharacteristicsHelper.GetPlanetTypeFactor(temperature, volatiles, substances);

            this.Quality = tempQuality * GoodWorldQuality;
            this.Name = $"{temperature}, {volatiles}, {substances}";
            this.ResourceAbundance = 100;

            double months = 12;
            this.MiningDifficulty = (Quality / GoodWorldQuality) / months;
        }
    }

    public enum ColonizationState : byte {
        Unknown, NonColonizable, NotColonized, Colonized
    }
}
