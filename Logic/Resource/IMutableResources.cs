namespace Logic.Resource {
    public interface IMutableResources : IComparableResources {
        new double CommonMetals { get; set; }
        new double Hydrogen { get; set; }
        new double RareEarthElements { get; set; }

        bool CanAdd(IBasicResources res);
        bool CanSubtract(IBasicResources res);

        void Add(IBasicResources res);
        void Subtract(IBasicResources res);
        void Multiply(double multiplier);

        void SetToZero();
    }
}