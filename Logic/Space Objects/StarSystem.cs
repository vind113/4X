using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Space_Objects {
    public class StarSystem {
        string name;
        List<Star> systemStars;
        List<Planet> systemPlanets;

        public StarSystem() {
            this.name = "DefaultSystem";
            this.systemStars = new List<Star>();
            this.systemPlanets = new List<Planet>();
        }

        public StarSystem(string name, List<Star> systemStars, List<Planet> systemPlanets) {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.systemStars = systemStars ?? throw new ArgumentNullException(nameof(systemStars));
            this.systemPlanets = systemPlanets ?? throw new ArgumentNullException(nameof(systemPlanets));
        }

        public string Name { get => this.name; set => this.name = value; }
        internal List<Star> SystemStars { get => this.systemStars; }
        internal List<Planet> SystemPlanets { get => this.systemPlanets; }

        public static StarSystem GetSolarSystem() {
            List<Planet> planets = new List<Planet>();

            planets.Add(new Planet("Mercury", 2440, PlanetType.Barren, 0));
            planets.Add(new Planet("Venus", 6051, PlanetType.Barren, 0));
            planets.Add(new Planet("Earth", 6371, PlanetType.Continental, 7_500_000_000d));
            planets.Add(new Planet("Mars", 3389, PlanetType.Desert, 0));

            List<Star> stars = new List<Star> { new Star() { Name = "Sun" } };

            StarSystem solarSystem = new StarSystem("Solar System", stars, planets);

            return solarSystem;
        }
    }
}
