using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Space_Objects {
    public class Star : CelestialBody {
        public override string ToString() {
            return $"It is a {this.name}";
        }
    }
}
