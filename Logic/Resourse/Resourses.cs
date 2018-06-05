using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Resourse {
    struct Resourses {
        double hydrogen;
        double commonMetals;
        double rareEarthElements;

        public Resourses(double hydrogen, double commonMetals, double rareEarthElements) {
            this.hydrogen = hydrogen;
            this.commonMetals = commonMetals;
            this.rareEarthElements = rareEarthElements;
        }
    }
}
