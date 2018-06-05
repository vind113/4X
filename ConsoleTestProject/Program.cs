using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameLogic.Space_Objects;

namespace ConsoleTestProject {
    class Program {
        static void Main(string[] args) {
            Planet planet = Planet.GeneratePlanet();
            Console.WriteLine(planet);
        }
    }
}
