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
                stars.Add(Star.GenerateStar($"{name} Star #{index + 1}"));
            }
        
            return stars;
        }

        /// <summary>
        /// Генерирует колекцию <see cref="Planet"/> системы
        /// </summary>
        /// <param name="systemName">
        /// Имя звездной системы, в которой находятся генерируемые планеты
        /// </param>
        /// <param name="stars">
        /// Коллекция звезд этой системы
        /// </param>
        /// <returns>
        /// <see cref="List{T}"/>, содержащий <see cref="Planet"/>
        /// </returns>
        private static List<Planet> GenerateSystemPlanets(string systemName, List<Star> stars) {
            List<Planet> planets = new List<Planet>();

            int planetCount = 0;

            string systemNameLocal = systemName;

            GenerateBarrenPlanets(systemNameLocal, ref planetCount, planets);
            GenerateHabitablePlanets(systemNameLocal, ref planetCount, stars, planets);
            GenerateGasGiantPlanets(systemNameLocal, ref planetCount, planets);

            return planets;
        }

        /// <summary>
        /// Генерирует безжизненные планеты системы
        /// </summary>
        /// <param name="nameTemplate">
        /// Имя звездной системы, в которой находятся генерируемые планеты
        /// </param>
        /// <param name="planetCount">
        /// Счетчик планет системы
        /// </param>
        /// <param name="planets">
        /// Коллекция <see cref="Planet"/> системы
        /// </param>
        private static void GenerateBarrenPlanets(string systemName, ref int planetCount, List<Planet> planets) {
            int barrenCount = HelperRandomFunctions.GetRandomInt(minBarrenCount, maxBarrenCount + 1);
            for (int index = 0; index < barrenCount; index++) {
                planets.Add(PlanetFactory.GetPlanet(GetPlanetName(systemName, planetCount), PlanetTypeVariants.Barren));
                planetCount++;
            }
        }

        /// <summary>
        /// Генерирует обитаемые планеты системы
        /// </summary>
        /// <param name="systemName">
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
        private static void GenerateHabitablePlanets(string systemName, ref int planetCount, List<Star> stars, List<Planet> planets) {
            LuminosityClass systemStarClass = stars[0].LumClass;

            if (systemStarClass == LuminosityClass.G
             || systemStarClass == LuminosityClass.K
             || systemStarClass == LuminosityClass.F) {

                int habitablePlanetCount = HelperRandomFunctions.GetRandomInt(minHabitableCount, maxHabitableCount + 1);
                for (int index = 0; index < habitablePlanetCount; index++) {
                    GenerateHabitablePlanet(systemName, planets, planetCount);
                    planetCount++;
                }
            }
        }

        /// <summary>
        /// Генерирует одну обитаемую планету
        /// </summary>
        /// <param name="systemName">
        /// Имя звездной системы, в которой находятся генерируемые планеты
        /// </param>
        /// <param name="planets">
        /// Коллекция <see cref="Planet"/> системы
        /// </param>
        /// <param name="planetCount">
        /// Счетчик планет системы
        /// </param>
        private static void GenerateHabitablePlanet(string systemName, List<Planet> planets, int planetCount) {
            int probabilityIndex = HelperRandomFunctions.GetRandomInt(1, maxPercents + 1);

            string planetName = GetPlanetName(systemName, planetCount);

            PlanetTypeVariants planetType;

            if (probabilityIndex <= 30) {
                planetType = PlanetTypeVariants.Continental;

            }
            else if (probabilityIndex <= 50) {
                planetType = PlanetTypeVariants.Tundra;

            }
            else if (probabilityIndex <= 70) {
                planetType = PlanetTypeVariants.Ocean;

            }
            else if (probabilityIndex <= 80) {
                planetType = PlanetTypeVariants.Paradise;

            }
            else if (probabilityIndex <= 90) {
                planetType = PlanetTypeVariants.Tropical;

            }
            else {
                planetType = PlanetTypeVariants.Desert;

            }

            planets.Add(PlanetFactory.GetPlanet(planetName, planetType));
        }

        /// <summary>
        /// Генерирует газовые планеты системы
        /// </summary>
        /// <param name="systemName">
        /// Имя звездной системы, в которой находятся генерируемые планеты
        /// </param>
        /// <param name="planetCount">
        /// Счетчик планет системы
        /// </param>
        /// <param name="planets">
        /// Коллекция <see cref="Planet"/> системы
        /// </param>
        private static void GenerateGasGiantPlanets(string systemName, ref int planetCount, List<Planet> planets) {
            int gasGiantCount = HelperRandomFunctions.GetRandomInt(minGasGiantCount, maxGasGiantCount + 1);
            for (int index = 0; index < gasGiantCount; index++) {
                planets.Add(PlanetFactory.GetPlanet(GetPlanetName(systemName, planetCount), PlanetTypeVariants.GasGiant));
                planetCount++;
            }
        }

        private static string GetPlanetName(string systemName, int planetCount) {
            return $"{systemName} #{planetCount}";
        }

        /// <summary>
        /// Создает объект, отображающий Солнечную систему
        /// </summary>
        /// <returns>
        /// Объект <see cref="StarSystem"/>, отображающий Солнечную систему
        /// </returns>
        public static StarSystem GetSolarSystem() {
            List<Planet> planets = new List<Planet>();

            planets.Add(new Planet("Mercury", 2440, PlanetTypeVariants.Barren, 0d));
            planets.Add(new Planet("Venus", 6051, PlanetTypeVariants.Barren, 0d));
            planets.Add(new Planet("Earth", 6371, PlanetTypeVariants.Continental, 20_000_000_000d));
            planets.Add(new Planet("Mars", 3389, PlanetTypeVariants.Barren, 0d));

            planets.Add(new Planet("Jupiter", 71_492, PlanetTypeVariants.GasGiant, 0d));
            planets.Add(new Planet("Saturn", 60_268, PlanetTypeVariants.GasGiant, 0d));
            planets.Add(new Planet("Uranus", 25_559, PlanetTypeVariants.GasGiant, 0d));
            planets.Add(new Planet("Neptune", 24_764, PlanetTypeVariants.GasGiant, 0d));

            List<Star> stars = new List<Star> { new Star("Sun", 696_392d, LuminosityClass.G) };

            StarSystem solarSystem = new StarSystem("Solar System", stars, planets);

            return solarSystem;
        }
    }
}
