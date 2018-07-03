using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using Logic.PlayerClasses;
using Logic.SupportClasses;

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
        public List<Star> SystemStars { get => this.systemStars; }
        public List<Planet> SystemPlanets { get => this.systemPlanets; }


        public int PlanetsCount {
            get {
                return this.SystemPlanets.Count;
            }
        }

        public int StarsCount {
            get {
                return this.SystemStars.Count;
            }
        }

        public int ColonizedCount {
            get {
                int count = 0;
                foreach (var planet in this.SystemPlanets) {
                    if (planet.Population > 0) {
                        count++;
                    }
                }
                return count;
            }
        }

        public double SystemPopulation {
            get {
                double population = 0;
                foreach (var planet in this.SystemPlanets) {
                    if (planet.Population > 0) {
                        population += planet.Population;
                    }
                }
                return population;
            }
        }

        public static StarSystem GetSolarSystem() {
            List<Planet> planets = new List<Planet>();

            planets.Add(new Planet("Mercury", 2440, PlanetTypeValue.Barren, 0d));
            planets.Add(new Planet("Venus", 6051, PlanetTypeValue.Barren, 0d));
            planets.Add(new Planet("Earth", 6371, PlanetTypeValue.Continental, 12_500_000_000d));
            planets.Add(new Planet("Mars", 3389, PlanetTypeValue.Desert, 0d));

            planets.Add(new Planet("Jupiter", 71_492, PlanetTypeValue.GasGiant, 0d));
            planets.Add(new Planet("Saturn", 60_268, PlanetTypeValue.GasGiant, 0d));
            planets.Add(new Planet("Uranus", 25_559, PlanetTypeValue.GasGiant, 0d));
            planets.Add(new Planet("Neptune", 24_764, PlanetTypeValue.GasGiant, 0d));

            List<Star> stars = new List<Star> { new Star("Sun", 696_392d, LuminosityClass.G) };

            StarSystem solarSystem = new StarSystem("Solar System", stars, planets);

            return solarSystem;
        }

        #region Generate Star System
        public static StarSystem GenerateSystem(string name) {

            List<Star> stars = GenerateSystemStars(name);

            List<Planet> planets = GenerateSystemPlanets(name, stars);

            return new StarSystem(name, stars, planets);
        }

        private static List<Star> GenerateSystemStars(string name) {
            List<Star> stars = new List<Star>();
            int starsCount = 0;
            int probabilityIndex = HelperRandomFunctions.GetRandomInt(1, 101);

            if (probabilityIndex < 55) {
                starsCount = 1;
            }
            else if (probabilityIndex < 100) {
                starsCount = 2;
            }
            else {
                starsCount = 3;
            }

            for (int index = 0; index < starsCount; index++) {
                stars.Add(Star.GenerateStar($"{name} Sun-{index+1}"));
            }
            
            return stars;
        }

        private static List<Planet> GenerateSystemPlanets(string name, List<Star> stars) {
            List<Planet> planets = new List<Planet>();

            int planetCount = 0;

            GenerateBarrenPlanets(name, ref planetCount, planets);
            GenerateHabitablePlanets(name, ref planetCount, stars, planets);
            GenerateGasGiantPlanets(name, ref planetCount, planets);

            return planets;
        }

        private static void GenerateBarrenPlanets(string name, ref int planetCount, List<Planet> planets) {
            int barrenCount = HelperRandomFunctions.GetRandomInt(0, 5);
            for (int index = 0; index < barrenCount; index++) {
                planets.Add(Planet.GeneratePlanet($"{name}-{planetCount}", PlanetTypeValue.Barren));
                planetCount++;
            }
        }

        private static void GenerateHabitablePlanets(string name, ref int planetCount, List<Star> stars, List<Planet> planets) {
            LuminosityClass systemStarClass = stars[0].LumClass;

            if (systemStarClass == LuminosityClass.G
             || systemStarClass == LuminosityClass.K
             || systemStarClass == LuminosityClass.F) {

                int habitablePlanetCount = HelperRandomFunctions.GetRandomInt(0, 2);
                for (int index = 0; index < habitablePlanetCount; index++) {
                    GenerateHabitablePlanet(name, planets, planetCount);
                    planetCount++;
                }
            }
        }

        public static void GenerateHabitablePlanet(string name, List<Planet> planets, int planetCount) {
            int probabilityIndex = HelperRandomFunctions.GetRandomInt(1, 101);

            if (probabilityIndex <= 30) {
                planets.Add(Planet.GeneratePlanet($"{name}-{planetCount}", PlanetTypeValue.Continental));

            }
            else if (probabilityIndex <= 50) {
                planets.Add(Planet.GeneratePlanet($"{name}-{planetCount}", PlanetTypeValue.Tundra));

            }
            else if (probabilityIndex <= 70) {
                planets.Add(Planet.GeneratePlanet($"{name}-{planetCount}", PlanetTypeValue.Ocean));

            }
            else if (probabilityIndex <= 80) {
                planets.Add(Planet.GeneratePlanet($"{name}-{planetCount}", PlanetTypeValue.Paradise));

            }
            else if (probabilityIndex <= 90) {
                planets.Add(Planet.GeneratePlanet($"{name}-{planetCount}", PlanetTypeValue.Tropical));

            }
            else {
                planets.Add(Planet.GeneratePlanet($"{name}-{planetCount}", PlanetTypeValue.Desert));
            }
        }

        private static void GenerateGasGiantPlanets(string name, ref int planetCount, List<Planet> planets) {
            int gasGiantCount = HelperRandomFunctions.GetRandomInt(0, 7);
            for (int index = 0; index < gasGiantCount; index++) {
                planets.Add(Planet.GeneratePlanet($"{name}-{planetCount}", PlanetTypeValue.GasGiant));
                planetCount++;
            }
        }
        #endregion

        public void NextTurn(Player player) {
            foreach(Planet planet in this.SystemPlanets) {
                planet.NextTurn(player);
            }
            foreach(Star star in this.SystemStars) {
                star.NextTurn();
            }
        }
    }
}
