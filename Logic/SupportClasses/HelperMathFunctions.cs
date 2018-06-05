using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.SupportClasses {
    public class HelperMathFunctions {
        public static double SphereArea(double radius) {
            return 4 * Math.PI * Math.Pow(radius, 2);
        }
    }
}
