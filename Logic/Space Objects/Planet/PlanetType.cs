using System;

namespace Logic.SpaceObjects {
    [Serializable]
    public struct PlanetType {
        private const double goodWorldQuality = 100;

        private double quality;
        private string name;
        private double miningDifficulty;
        private byte resourseAbundance;

        private TemperatureClass temperatureClass;
        private VolatilesClass volatilesClass;
        private SubstancesClass substancesClass;

        public PlanetType(TemperatureClass temperature, VolatilesClass volatiles, SubstancesClass substances) {
            this.temperatureClass = temperature;
            this.volatilesClass = volatiles;
            this.substancesClass = substances;

            double tempQuality = PlanetCharacteristicsHelper.GetPlanetTypeFactor(temperature, volatiles, substances);

            this.quality = tempQuality * GoodWorldQuality;
            this.name = $"{temperature}, {volatiles}, {substances}";
            this.resourseAbundance = 100;

            double months = 12;
            this.miningDifficulty = (quality / PlanetType.GoodWorldQuality) / months;
        }

        public double Quality { get => this.quality; }
        public string Name { get => this.name; }

        public static double GoodWorldQuality { get => goodWorldQuality; }
        public double MiningDifficulty { get => this.miningDifficulty; }
        public byte ResourseAbundance { get => this.resourseAbundance; }

        public TemperatureClass TemperatureClass { get => this.temperatureClass; }
        public VolatilesClass VolatilesClass { get => this.volatilesClass; }
        public SubstancesClass SubstancesClass { get => this.substancesClass; }
    }

    public enum ColonizationState : byte {
        Unknown, NonColonizable, NotColonized, Colonized
    }
}
