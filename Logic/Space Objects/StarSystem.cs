using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Logic.PlayerClasses;
using Logic.Resourse;
using Logic.SupportClasses;

namespace Logic.SpaceObjects {
    /// <summary>
    ///     Представляет звездную систему
    /// </summary>
    public class StarSystem : INotifyPropertyChanged {
        private string name;

        private readonly List<Star> systemStars;
        private readonly List<Planet> systemPlanets;

        private Resourses systemResourses;
        private int minersCount;

        private byte colonizedCount;
        private double population;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Planet_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if(e.PropertyName == nameof(Planet.IsColonized) && sender is Planet planet) {
                this.ColonizedCount += (byte)((planet.IsColonized) ? 1 : -1);
            }
        }

        private void StarChangedHandler(object sender, PropertyChangedEventArgs e) {

        }

        /// <summary>
        ///     Инициализирует экземпляр класса с значениями по умолчанию
        /// </summary>
        public StarSystem() {
            
        }

        /// <summary>
        ///     Инициализирует экземпляр класса звездной системы
        /// </summary>
        /// <param name="name"></param>
        /// <param name="systemStars"></param>
        /// <param name="planets"></param>
        public StarSystem(string name, IList<Star> stars, IList<Planet> planets) {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.systemStars = new List<Star>(stars) ?? throw new ArgumentNullException(nameof(stars));

            if (planets.Count > 255) {
                throw new ArgumentOutOfRangeException(nameof(planets), "Count can't be greater than 255");
            }

            this.systemPlanets = new List<Planet>(planets) ?? throw new ArgumentNullException(nameof(planets));

            foreach (var planet in this.SystemPlanets) {
                planet.PropertyChanged += this.Planet_PropertyChanged;
            }

            this.SystemPopulation = this.SetSystemPopulation();

            if(this.SystemPopulation > 0) {
                this.MinersCount = 10;
            }

            this.ColonizedCount = this.SetColonizedPlantes();
            this.SystemResourses = this.SetResourses();
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
        public int PlanetsCount { get => this.SystemPlanets.Count; }

        /// <summary>
        /// Возвращает количество колонизируемых планет в системе
        /// </summary>
        public int HabitablePlanets { get => this.GetHabitableCount(); }

        /// <summary>
        /// Возвращает количество звезд в системе
        /// </summary>
        public int StarsCount { get => this.SystemStars.Count; }

        /// <summary>
        /// Возвращает имя звездной системы
        /// </summary>
        public string Name {
            get => this.name;
            set {
                if (this.name != value) {
                    this.name = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Возвращает количество колонизированных планет в системе
        /// </summary>
        public byte ColonizedCount {
            get => colonizedCount;
            private set {
                if (this.colonizedCount != value) {
                    this.colonizedCount = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Возвращает количество обитателей системы
        /// </summary>
        public double SystemPopulation {
            get => population;
            private set {
                if (this.population != value) {
                    this.population = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Возвращает ресурсы системы
        /// </summary>
        public Resourses SystemResourses {
            get => this.systemResourses;
            private set => this.systemResourses = value;
        }

        /// <summary>
        /// Возвращает количество добывающих кораблей системы
        /// </summary>
        public int MinersCount {
            get => this.minersCount;
            private set {
                this.minersCount = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Выполняет все операции для перехода на следующий ход
        /// </summary>
        /// <param name="player">
        ///     Игрок, которому принадлежит система
        /// </param>
        public void NextTurn(Player player) {

            foreach (Planet planet in this.SystemPlanets) {
                planet.NextTurn(player);
            }

            foreach (Star star in this.SystemStars) {
                star.NextTurn();
            }

            this.SystemPopulation = this.SetSystemPopulation();

            MineSystemResourses(player);
        }

        private void MineSystemResourses(Player player) {
            if(this.SystemPopulation == 0) {
                return;
            }

            Miner.Mine(this.MinersCount, this.SystemResourses, player.OwnedResourses);
        }

        private double SetSystemPopulation() {
            double population = 0;

            foreach (var planet in this.SystemPlanets) {
                if (planet.IsColonized) {
                    population += planet.Population;
                }
            }

            return population;
        }

        private byte SetColonizedPlantes() {
            byte colonized = 0;

            foreach (var planet in this.SystemPlanets) {
                if (planet.IsColonized) {
                    colonized++;
                }
            }

            return colonized;
        }

        private int GetHabitableCount() {
            int habitableCount = 0;
            foreach (var planet in this.SystemPlanets) {
                if (planet.MaximumPopulation > 0) {
                    habitableCount++;
                }
            }
            return habitableCount;
        }

        private Resourses SetResourses() {
            double hydrogen = HelperRandomFunctions.GetRandomDouble() * 1E22;
            double commonMetals = HelperRandomFunctions.GetRandomDouble() * 1E24;
            double rareElements = HelperRandomFunctions.GetRandomDouble() * 1E20;

            return new Resourses(hydrogen, commonMetals, rareElements);
        }
    }
}
