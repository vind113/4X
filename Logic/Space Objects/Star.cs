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

        public Star(string name, double radius, LuminosityClass luminosityClass) {
            this.name = name;
            this.radius = radius;

            this.lumClass = luminosityClass;

            this.area = HelperMathFunctions.SphereArea(this.Radius);
        }

        //тестируй
        public static Star GenerateStar(string name) {
            int radius = 0;
            LuminosityClass luminosityClass;

            double starFraction = HelperRandomFunctions.GetRandomDouble();

            if (starFraction < 0.0003) {
                radius = HelperRandomFunctions.GetRandomInt(4_620_000, 10_000_000);
                luminosityClass = LuminosityClass.O;

            }
            else if (starFraction < 0.0013) {
                radius = HelperRandomFunctions.GetRandomInt(1_260_000, 4_620_000);
                luminosityClass = LuminosityClass.B;

            }
            else if (starFraction < 0.006) {
                radius = HelperRandomFunctions.GetRandomInt(805_000, 1_260_000);
                luminosityClass = LuminosityClass.A;

            }
            else if (starFraction < 0.03) {
                radius = HelperRandomFunctions.GetRandomInt(728_000, 805_000);
                luminosityClass = LuminosityClass.F;

            }
            //реальное соотношение 0.076
            else if (starFraction < 0.1) {
                radius = HelperRandomFunctions.GetRandomInt(672_000, 728_000);
                luminosityClass = LuminosityClass.G;

            }
            //реальное соотношение 0.12
            else if (starFraction < 0.20) {
                radius = HelperRandomFunctions.GetRandomInt(490_000, 672_000);
                luminosityClass = LuminosityClass.K;

            }
            else {
                radius = HelperRandomFunctions.GetRandomInt(360_000, 490_000);
                luminosityClass = LuminosityClass.M;
            }

            return new Star(name, radius, luminosityClass);
        }

        public override string ToString() {
            return $"It is a star called {this.name}" +
                $" with radius of {this.Radius:E4} km and area of {this.Area:E4} km^2." +
                $"Luminosity class is {this.LumClass}";
        }

        public void NextTurn() {

        }

        /*
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
        */
    }
}
