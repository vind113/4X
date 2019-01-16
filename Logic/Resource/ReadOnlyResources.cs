namespace Logic.Resource {
    public class ReadOnlyResources : IComparableResources {
        private IComparableResources innerResources;

        public ReadOnlyResources() {
            innerResources = new Resources();
        }

        public ReadOnlyResources(double hydrogen, double commonMetals, double rareEarthMetals) {
            innerResources = new Resources(hydrogen, commonMetals, rareEarthMetals);
        }

        public ReadOnlyResources(IBasicResources res) {
            innerResources = new Resources(res);
        }

        public double CommonMetals => innerResources.CommonMetals;
        public double Hydrogen => innerResources.Hydrogen;
        public double RareEarthElements => innerResources.RareEarthElements;

        public bool IsEqual(IBasicResources res) {
            return innerResources.IsEqual(res);
        }

        public bool IsNotEqual(IBasicResources res) {
            return innerResources.IsNotEqual(res);
        }

        public bool IsStrictlyGreater(IBasicResources res) {
            return innerResources.IsStrictlyGreater(res);
        }
    }
}
