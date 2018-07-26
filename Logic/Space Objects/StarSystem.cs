using System;
using System.Collections.Generic;
using Logic.PlayerClasses;

namespace Logic.Space_Objects {
    /// <summary>
    ///     Представляет звездную систему
    /// </summary>
    public class StarSystem {
        string name;
        List<Star> systemStars;
        List<Planet> systemPlanets;

        /// <summary>
        ///     Инициализирует экземпляр класса с значениями по умолчанию
        /// </summary>
        public StarSystem() {
            this.name = "DefaultSystem";
            this.systemStars = new List<Star>();
            this.systemPlanets = new List<Planet>();
        }

        /// <summary>
        ///     Инициализирует экземпляр класса звездной системы
        /// </summary>
        /// <param name="name"></param>
        /// <param name="systemStars"></param>
        /// <param name="systemPlanets"></param>
        public StarSystem(string name, List<Star> systemStars, List<Planet> systemPlanets) {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.systemStars = systemStars ?? throw new ArgumentNullException(nameof(systemStars));
            this.systemPlanets = systemPlanets ?? throw new ArgumentNullException(nameof(systemPlanets));
        }

        /// <summary>
        /// Возвращает имя звездной системы
        /// </summary>
        public string Name {
            get => this.name;
            set => this.name = value;
        }

        /// <summary>
        /// Возвращает ссылку на коллекцию объектов <see cref="Star"/> системы
        /// </summary>
        public List<Star> SystemStars { get => this.systemStars; }

        /// <summary>
        /// Возвращает ссылку на коллекцию объектов <see cref="Planet"/> системы
        /// </summary>
        public List<Planet> SystemPlanets { get => this.systemPlanets; }

        /// <summary>
        /// Возвращает количество планет в системе
        /// </summary>
        public int PlanetsCount {
            get {
                return this.SystemPlanets.Count;
            }
        }

        /// <summary>
        /// Возвращает количество звезд в системе
        /// </summary>
        public int StarsCount {
            get {
                return this.SystemStars.Count;
            }
        }

        /// <summary>
        /// Возвращает количество колонизированных планет в системе
        /// </summary>
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

        /// <summary>
        /// Возвращает количество обитателей системы
        /// </summary>
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

        /// <summary>
        /// Возвращает количество колонизируемых планет в системе
        /// </summary>
        public int HabitablePlanets {
            get {
                int habitableCount = 0;
                foreach (var planet in this.SystemPlanets) {
                    if (planet.MaximumPopulation > 0) {
                        habitableCount++;
                    }
                }
                return habitableCount;
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

        /// <summary>
        ///     Выполняет все операции для перехода на следующий ход
        /// </summary>
        /// <param name="player">
        ///     Игрок, которому принадлежит система
        /// </param>
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
