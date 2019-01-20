using Logic.Resource;
using Logic.SupportClasses;

namespace Logic.SpaceObjects {
    public class StarSystemResourceGenerator {
        public Resources GenerateResources() {
            double hydrogen = HelperRandomFunctions.GetRandomDouble() * 1E22;
            double commonMetals = HelperRandomFunctions.GetRandomDouble() * 1E24;
            double rareElements = HelperRandomFunctions.GetRandomDouble() * 1E20;

            return new Resources(hydrogen, commonMetals, rareElements);
        }
    }
}
