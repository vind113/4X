using System;

using Logic.Space_Objects;

namespace ConsoleTestProject {
    class Program {
        static void Main(string[] args) {

            Planet planet = new Planet("ACX", 9000, PlanetType.Continental, 1_000_000);
            Console.WriteLine(planet);

            for (int i = 0; i < 2400; i++) {
                planet.NextTurn();
                Console.WriteLine($"{planet.Population}");
            }
        }
    }
}
