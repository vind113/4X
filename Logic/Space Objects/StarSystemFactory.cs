using Logic.SupportClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.SpaceObjects {
    public class StarSystemFactory {
        private const int minBarrenCount = 0;
        private const int maxBarrenCount = 4;

        private const int minHabitableCount = 0;
        private const int maxHabitableCount = 2;

        private const int minGasGiantCount = 0;
        private const int maxGasGiantCount = 6;

        private const int maxPercents = 100;

        /// <summary>
        /// Генерирует <see cref="StarSystem"/> с заданым именем
        /// </summary>
        /// <param name="name">
        /// Имя звездной системы
        /// </param>
        /// <returns>
        /// Объект <see cref="StarSystem"/>
        /// </returns>
        public static StarSystem GetStarSystem(string name) {

            List<Star> stars = GenerateSystemStars(name);

            List<Planet> planets = GenerateSystemPlanets(name, stars);

            return new StarSystem(name, stars, planets);
        }

        /// <summary>
        /// Генерирует колекцию <see cref="Star"/> системы
        /// </summary>
        /// <param name="name">
        /// Имя звездной системы, в которой находятся генерируемые звезды
        /// </param>
        /// <returns>
        /// <see cref="List{T}"/>, содержащий <see cref="Star"/>
        /// </returns>
        private static List<Star> GenerateSystemStars(string name) {
            List<Star> stars = new List<Star>();
            int starsCount = 0;
            int probabilityIndex = HelperRandomFunctions.GetRandomInt(1, maxPercents + 1);

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
                stars.Add(Star.GenerateStar($"{name} Sun #{index + 1}"));
            }
        
            return stars;
        }

        /// <summary>
        /// Генерирует колекцию <see cref="Planet"/> системы
        /// </summary>
        /// <param name="name">
        /// Имя звездной системы, в которой находятся генерируемые планеты
        /// </param>
        /// <param name="stars">
        /// Коллекция звезд этой системы
        /// </param>
        /// <returns>
        /// <see cref="List{T}"/>, содержащий <see cref="Planet"/>
        /// </returns>
        private static List<Planet> GenerateSystemPlanets(string name, List<Star> stars) {
            List<Planet> planets = new List<Planet>();

            int planetCount = 0;

            GenerateBarrenPlanets(name, ref planetCount, planets);
            GenerateHabitablePlanets(name, ref planetCount, stars, planets);
            GenerateGasGiantPlanets(name, ref planetCount, planets);

            return planets;
        }

        /// <summary>
        /// Генерирует безжизненные планеты системы
        /// </summary>
        /// <param name="name">
        /// Имя звездной системы, в которой находятся генерируемые планеты
        /// </param>
        /// <param name="planetCount">
        /// Счетчик планет системы
        /// </param>
        /// <param name="planets">
        /// Коллекция <see cref="Planet"/> системы
        /// </param>
        private static void GenerateBarrenPlanets(string name, ref int planetCount, List<Planet> planets) {
            int barrenCount = HelperRandomFunctions.GetRandomInt(minBarrenCount, maxBarrenCount + 1);
            for (int index = 0; index < barrenCount; index++) {
                planets.Add(PlanetFactory.GetPlanet($"{name} #{planetCount}(B)", PlanetTypeValue.Barren));
                planetCount++;
            }
        }

        /// <summary>
        /// Генерирует обитаемые планеты системы
        /// </summary>
        /// <param name="name">
        /// Имя звездной системы, в которой находятся генерируемые планеты
        /// </param>
        /// <param name="planetCount">
        /// Счетчик планет системы
        /// </param>
        /// <param name="stars">
        /// Коллекция <see cref="Star"/> системы
        /// </param>
        /// <param name="planets">
        /// Коллекция <see cref="Planet"/> системы
        /// </param>
        private static void GenerateHabitablePlanets(string name, ref int planetCount, List<Star> stars, List<Planet> planets) {
            LuminosityClass systemStarClass = stars[0].LumClass;

            if (systemStarClass == LuminosityClass.G
             || systemStarClass == LuminosityClass.K
             || systemStarClass == LuminosityClass.F) {

                int habitablePlanetCount = HelperRandomFunctions.GetRandomInt(minHabitableCount, maxHabitableCount + 1);
                for (int index = 0; index < habitablePlanetCount; index++) {
                    GenerateHabitablePlanet(name, planets, planetCount);
                    planetCount++;
                }
            }
        }

        /// <summary>
        /// Генерирует одну обитаемую планету
        /// </summary>
        /// <param name="name">
        /// Имя звездной системы, в которой находятся генерируемые планеты
        /// </param>
        /// <param name="planets">
        /// Коллекция <see cref="Planet"/> системы
        /// </param>
        /// <param name="planetCount">
        /// Счетчик планет системы
        /// </param>
        private static void GenerateHabitablePlanet(string name, List<Planet> planets, int planetCount) {
            int probabilityIndex = HelperRandomFunctions.GetRandomInt(1, maxPercents + 1);

            string planetName = $"{name} #{planetCount}(H)";

            if (probabilityIndex <= 30) {
                planets.Add(PlanetFactory.GetPlanet(planetName, PlanetTypeValue.Continental));

            }
            else if (probabilityIndex <= 50) {
                planets.Add(PlanetFactory.GetPlanet(planetName, PlanetTypeValue.Tundra));

            }
            else if (probabilityIndex <= 70) {
                planets.Add(PlanetFactory.GetPlanet(planetName, PlanetTypeValue.Ocean));

            }
            else if (probabilityIndex <= 80) {
                planets.Add(PlanetFactory.GetPlanet(planetName, PlanetTypeValue.Paradise));

            }
            else if (probabilityIndex <= 90) {
                planets.Add(PlanetFactory.GetPlanet(planetName, PlanetTypeValue.Tropical));

            }
            else {
                planets.Add(PlanetFactory.GetPlanet(planetName, PlanetTypeValue.Desert));
            }
        }

        /// <summary>
        /// Генерирует газовые планеты системы
        /// </summary>
        /// <param name="name">
        /// Имя звездной системы, в которой находятся генерируемые планеты
        /// </param>
        /// <param name="planetCount">
        /// Счетчик планет системы
        /// </param>
        /// <param name="planets">
        /// Коллекция <see cref="Planet"/> системы
        /// </param>
        private static void GenerateGasGiantPlanets(string name, ref int planetCount, List<Planet> planets) {
            int gasGiantCount = HelperRandomFunctions.GetRandomInt(minGasGiantCount, maxGasGiantCount + 1);
            for (int index = 0; index < gasGiantCount; index++) {
                planets.Add(PlanetFactory.GetPlanet($"{name} #{planetCount}(GG)", PlanetTypeValue.GasGiant));
                planetCount++;
            }
        }

        /// <summary>
        /// Создает объект, отображающий Солнечную систему
        /// </summary>
        /// <returns>
        /// Объект <see cref="StarSystem"/>, отображающий Солнечную систему
        /// </returns>
        public static StarSystem GetSolarSystem() {
            List<Planet> planets = new List<Planet>();

            planets.Add(new Planet("Mercury", 2440, PlanetTypeValue.Barren, 0d));
            planets.Add(new Planet("Venus", 6051, PlanetTypeValue.Barren, 0d));
            planets.Add(new Planet("Earth", 6371, PlanetTypeValue.Continental, 20_000_000_000d));
            planets.Add(new Planet("Mars", 3389, PlanetTypeValue.Desert, 0d));

            planets.Add(new Planet("Jupiter", 71_492, PlanetTypeValue.GasGiant, 0d));
            planets.Add(new Planet("Saturn", 60_268, PlanetTypeValue.GasGiant, 0d));
            planets.Add(new Planet("Uranus", 25_559, PlanetTypeValue.GasGiant, 0d));
            planets.Add(new Planet("Neptune", 24_764, PlanetTypeValue.GasGiant, 0d));

            List<Star> stars = new List<Star> { new Star("Sun", 696_392d, LuminosityClass.G) };

            StarSystem solarSystem = new StarSystem("Solar System", stars, planets);

            return solarSystem;
        }
    }
}
