namespace Logic.Resource {
    public interface IResources {
        double CommonMetals { get; set; }
        double Hydrogen { get; set; }
        double RareEarthElements { get; set; }

        void Add(Resources res);
        void Subtract(Resources res);
        void Multiply(double multiplier);

        void SetToZero();

        bool CanAdd(Resources res);
        bool CanSubtract(Resources res);

        bool IsStrictlyGreater(Resources res);
        bool IsEqual(Resources res);
        bool IsNotEqual(Resources res);
    }
}