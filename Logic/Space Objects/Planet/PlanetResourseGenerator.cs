using Logic.Resource;

namespace Logic.SpaceObjects.PlanetClasses {
    public class PlanetResourceGenerator {
        public PlanetResourceGenerator() {

        }

        public Resources GenerateFor(Planet planet) {
            double commonMetals = 0;
            double rareEarthElements = 0;
            double hydrogen = 0;

            double massOfTenKmCrust = (10d * ((3d) * 10E9));

            if (planet.Type.SubstancesClass == SubstancesClass.Terra) {
                commonMetals = planet.Area * (massOfTenKmCrust / 20);
                rareEarthElements = planet.Area * (massOfTenKmCrust / 1E5);
                hydrogen = planet.Area * (massOfTenKmCrust / 200);

            }
            else if (planet.Type.SubstancesClass == SubstancesClass.Ferria) {
                commonMetals = planet.Area * (massOfTenKmCrust / 2);
                rareEarthElements = planet.Area * (massOfTenKmCrust / 1E4);
                hydrogen = planet.Area * (massOfTenKmCrust / 200);

            }
            else if (planet.Type.SubstancesClass == SubstancesClass.Jupiter) {
                commonMetals = 0;
                rareEarthElements = 0;
                hydrogen = planet.Area * (1.4 * 20 * 1E3 * 1E9);

            }

            return new Resources(hydrogen, commonMetals, rareEarthElements);
        }
    }
}
