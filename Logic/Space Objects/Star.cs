using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Logic.SupportClasses;

namespace Logic.Space_Objects {
    public enum LuminosityClass {
        O, B, A, F, G, K, M
    }

    public class Star : CelestialBody {
        private LuminosityClass lumClass;

        public LuminosityClass LumClass { get => this.lumClass; }

        public Star() {

        }

        public Star(string name, double radius) {
            this.name = name;
            this.radius = radius;

            this.lumClass = this.GetLuminosityClass(radius);

            this.area = HelperMathFunctions.SphereArea(this.Radius);
        }

        public override string ToString() {
            return $"It is a star called {this.name}" +
                $" with radius of {this.Radius:E4} km and area of {this.Area:E4} km." +
                $"Luminosity class is {this.LumClass}";
        }

        public void NextTurn() {

        }

        private LuminosityClass GetLuminosityClass(double starRadius) {

            double sunRadius = 696_392d;

            if (starRadius <= sunRadius * 0.7) return LuminosityClass.M;
            if (starRadius > sunRadius * 0.7 && starRadius <= sunRadius * 0.96) return LuminosityClass.K;
            if (starRadius > sunRadius * 0.96 && starRadius <= sunRadius * 1.15) return LuminosityClass.G;
            if (starRadius > sunRadius * 1.15 && starRadius <= sunRadius * 1.4) return LuminosityClass.F;
            if (starRadius > sunRadius * 1.4 && starRadius <= sunRadius * 1.8) return LuminosityClass.A;
            if (starRadius > sunRadius * 1.8 && starRadius <= sunRadius * 6.6) return LuminosityClass.B;

            return LuminosityClass.O;
        }
    }
}
