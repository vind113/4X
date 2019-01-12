using Logic.SupportClasses;
using System.Collections.Generic;

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

            if (probabilityIndex < 70) {
                starsCount = 1;
            }
            else if (probabilityIndex < 100) {
                starsCount = 2;
            }
            else {
                starsCount = 3;
            }

            for (int index = 0; index < starsCount; index++) {
                stars.Add(StarFactory.GenerateStar($"{name} Star #{index + 1}"));
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

            planets.AddRange(GetBarrenPlanets(systemNameLocal, ref planetCount, stars));
            planets.AddRange(GetHabitablePlanets(systemNameLocal, ref planetCount, stars));
            planets.AddRange(GetGasGiantPlanets(systemNameLocal, ref planetCount));

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
        /// <param name="stars">
        /// Коллекция <see cref="Star"/> системы
        /// </param>
        private static List<Planet> GetBarrenPlanets(string systemName, ref int planetCount, List<Star> stars) {
            int barrenCount = HelperRandomFunctions.GetRandomInt(minBarrenCount, maxBarrenCount + 1);

            LuminosityClass systemStarClass = stars[0].LumClass;
            List<Planet> planets = new List<Planet>();

            TemperatureClass planetTemperature;

            if (systemStarClass == LuminosityClass.M
             || systemStarClass == LuminosityClass.K) {

                planetTemperature = TemperatureClass.Warm;
            }
            else { 
                planetTemperature = TemperatureClass.Hot;
            }

            if (HelperRandomFunctions.PercentProbableBool(20) && barrenCount >= 1) {
                planets.Add(PlanetFactory.GetPlanet(GetPlanetName(systemName, planetCount),
                    new PlanetType(planetTemperature, VolatilesClass.Airless, SubstancesClass.Ferria)));

                planetCount++;
                barrenCount--;

                if (HelperRandomFunctions.PercentProbableBool(20) && barrenCount >= 1) {
                    planets.Add(PlanetFactory.GetPlanet(GetPlanetName(systemName, planetCount),
                        new PlanetType(planetTemperature, VolatilesClass.Airless, SubstancesClass.Ferria)));

                    planetCount++;
                    barrenCount--;
                }
            }
           
            for (int index = 0; index < barrenCount; index++) {
                planets.Add(PlanetFactory.GetPlanet(GetPlanetName(systemName, planetCount),
                    new PlanetType(planetTemperature, VolatilesClass.Airless, SubstancesClass.Terra)));
                planetCount++;
            }

            return planets;
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
        private static List<Planet> GetHabitablePlanets(string systemName, ref int planetCount, List<Star> stars) {
            LuminosityClass systemStarClass = stars[0].LumClass;
            List<Planet> planets = new List<Planet>();

            if (systemStarClass == LuminosityClass.G
             || systemStarClass == LuminosityClass.K
             || systemStarClass == LuminosityClass.F) {

                int habitablePlanetCount = HelperRandomFunctions.GetRandomInt(minHabitableCount, maxHabitableCount + 1);
                for (int index = 0; index < habitablePlanetCount; index++) {
                    planets.Add(GetHabitablePlanet(GetPlanetName(systemName, planetCount), planetCount, systemStarClass));
                    planetCount++;
                }
            }

            return planets;
        }

        /// <summary>
        /// Генерирует одну обитаемую планету
        /// </summary>
        /// <param name="systemName">
        /// Имя звездной системы, в которой находятся генерируемые планеты
        /// </param>
        private static HabitablePlanet GetHabitablePlanet(string planetName, int planetCount, LuminosityClass mainStarClass) {
            int probabilityIndex = HelperRandomFunctions.GetRandomInt(1, maxPercents + 1);

            PlanetType planetType;

            if (probabilityIndex >= 90) {
                planetType = new PlanetType(TemperatureClass.Temperate, VolatilesClass.Marine, SubstancesClass.Terra);

            }
            else if (probabilityIndex >= 70) {
                planetType = new PlanetType(TemperatureClass.Cool, VolatilesClass.Marine, SubstancesClass.Terra);

            }
            else {
                planetType = new PlanetType(TemperatureClass.Cool, VolatilesClass.Desertic, SubstancesClass.Terra);

            }

            return PlanetFactory.GetHabitablePlanet(planetName, planetType);
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
        private static List<Planet> GetGasGiantPlanets(string systemName, ref int planetCount) {
            int gasGiantCount = HelperRandomFunctions.GetRandomInt(minGasGiantCount, maxGasGiantCount + 1);
            List<Planet> planets = new List<Planet>();

            for (int index = 0; index < gasGiantCount; index++) {
                planets.Add(PlanetFactory.GetPlanet(GetPlanetName(systemName, planetCount),
                    new PlanetType(TemperatureClass.Cold, VolatilesClass.Airless, SubstancesClass.Jupiter)));
                planetCount++;
            }

            return planets;
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

            planets.Add(new Planet("Mercury", 2440, new PlanetType(TemperatureClass.Hot, VolatilesClass.Airless, SubstancesClass.Ferria)));
            planets.Add(new Planet("Venus", 6051, new PlanetType(TemperatureClass.Hot, VolatilesClass.Desertic, SubstancesClass.Terra)));
            planets.Add(new HabitablePlanet("Earth", 6371, new PlanetType(TemperatureClass.Temperate,VolatilesClass.Marine, SubstancesClass.Terra), 20_000_000_000));
            planets.Add(new HabitablePlanet("Mars", 3389, new PlanetType(TemperatureClass.Cool, VolatilesClass.Desertic, SubstancesClass.Terra), 0));

            planets.Add(new Planet("Jupiter", 71_492, new PlanetType(TemperatureClass.Cold, VolatilesClass.Airless, SubstancesClass.Jupiter)));
            planets.Add(new Planet("Saturn", 60_268, new PlanetType(TemperatureClass.Cold, VolatilesClass.Airless, SubstancesClass.Jupiter)));
            planets.Add(new Planet("Uranus", 25_559, new PlanetType(TemperatureClass.Frigid, VolatilesClass.Airless, SubstancesClass.Jupiter)));
            planets.Add(new Planet("Neptune", 24_764, new PlanetType(TemperatureClass.Frigid, VolatilesClass.Airless, SubstancesClass.Jupiter)));

            List<Star> stars = new List<Star> { new Star("Sun", 696_392d, LuminosityClass.G) };

            StarSystem solarSystem = new StarSystem("Solar System", stars, planets);

            return solarSystem;
        }
    }
}
