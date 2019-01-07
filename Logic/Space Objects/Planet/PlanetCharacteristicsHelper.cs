using System;

namespace Logic.SpaceObjects {
    public class PlanetCharacteristicsHelper {

        public static double GetPlanetTypeFactor(
            TemperatureClass temperature, VolatilesClass volatiles, SubstancesClass substances) {

            return (GetTemperatureFactor(temperature) * GetVolatilesFactor(volatiles) * GetSubstancesFactor(substances));
        }

        private static double GetTemperatureFactor(TemperatureClass temperatureClass) {
            switch (temperatureClass) {
                case (TemperatureClass.Frigid):
                    return 0;
                case (TemperatureClass.Cold):
                    return 0.01;
                case (TemperatureClass.Cool):
                    return 0.9;
                case (TemperatureClass.Temperate):
                    return 1.1;
                case (TemperatureClass.Warm):
                    return 0.01;
                case (TemperatureClass.Hot):
                    return 0;
                default:
                    throw new ArgumentException($"{temperatureClass.ToString()} class is not acceptable");
            }
        }

        private static double GetVolatilesFactor(VolatilesClass volatilesClass) {
            switch (volatilesClass) {
                case (VolatilesClass.Airless):
                    return 0;
                case (VolatilesClass.Desertic):
                    return 0.01;
                case (VolatilesClass.Lacustrine):
                    return 0.05;
                case (VolatilesClass.Marine):
                    return 1.2;
                case (VolatilesClass.Oceanic):
                    return 0.3;
                case (VolatilesClass.Superoceanic):
                    return 0.1;
                default:
                    throw new ArgumentException($"{volatilesClass.ToString()} class is not acceptable");
            }
        }

        private static double GetSubstancesFactor(SubstancesClass substancesClass) {
            switch (substancesClass) {
                case (SubstancesClass.Ferria):
                    return 0.7;
                case (SubstancesClass.Terra):
                    return 1;
                case (SubstancesClass.Jupiter):
                    return 0;
                default:
                    throw new ArgumentException($"{substancesClass.ToString()} class is not acceptable");
            }
        }
    }
}
