using System.ComponentModel;

namespace Logic.Resource {
    public interface IResources {
        double CommonMetals { get; set; }
        double Hydrogen { get; set; }
        double RareEarthElements { get; set; }

        void Add(Resources res);
        void Subtract(Resources res);

        bool CanAdd(Resources res);
        bool CanSubtract(Resources res);

        bool IsStrictlyGreater(Resources res);
        void Multiply(double multiplier);
        void SetToZero();
    }
}