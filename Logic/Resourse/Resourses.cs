namespace Logic.Resourse {
    public struct Resourses {
        double hydrogen;
        double commonMetals;
        double rareEarthElements;

        public Resourses(double hydrogen, double commonMetals, double rareEarthElements) {
            this.hydrogen = hydrogen;
            this.commonMetals = commonMetals;
            this.rareEarthElements = rareEarthElements;
        }

        public double Hydrogen { get => this.hydrogen; set => this.hydrogen = value; }
        public double CommonMetals { get => this.commonMetals; set => this.commonMetals = value; }
        public double RareEarthElements { get => this.rareEarthElements; set => this.rareEarthElements = value; }

    }
}
