using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Logic.PlayerClasses;
using Logic.Resource;
using Logic.SupportClasses;
using Logic.Buildings;

namespace Logic.SpaceObjects {
    /// <summary>
    ///     Представляет звездную систему
    /// </summary>
    [Serializable]
    public class StarSystem : INotifyPropertyChanged {
        private string name;

        private readonly List<Star> systemStars;
        private readonly List<Planet> systemPlanets;

        private readonly SystemBuildings buildings;

        private Resources systemResources;
        private int minersCount;

        private byte colonizedCount;
        private long population;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Planet_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if(e.PropertyName == nameof(HabitablePlanet.IsColonized) && sender is HabitablePlanet planet) {
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

            this.buildings = new SystemBuildings();

            foreach (var planet in this.SystemPlanets) {
                planet.PropertyChanged += this.Planet_PropertyChanged;
            }

            this.SystemPopulation = this.SetSystemPopulation();

            if(this.SystemPopulation > 0) {
                this.MinersCount = 10;
            }

            this.ColonizedCount = this.SetColonizedPlantes();
            this.SystemResources = this.SetResources();
        }

        /// <summary>
        /// Возвращает ссылку на коллекцию объектов <see cref="Star"/> системы
        /// </summary>
        //public List<Star> SystemStars { get => this.systemStars; }
        public ReadOnlyCollection<Star> SystemStars { get => new ReadOnlyCollection<Star>(this.systemStars); }

        /// <summary>
        /// Возвращает ссылку на коллекцию объектов <see cref="Planet"/> системы
        /// </summary>
        public ReadOnlyCollection<Planet> SystemPlanets { get => new ReadOnlyCollection<Planet>(this.systemPlanets); }

        /// <summary>
        /// Возвращает ссылку на коллекцию объектов <see cref="HabitablePlanet"/> системы
        /// </summary>
        public ReadOnlyCollection<HabitablePlanet> SystemHabitablePlanets {
            get {
                List<HabitablePlanet> habitablePlanets = new List<HabitablePlanet>(); 

                foreach (var planet in this.SystemPlanets) {
                    if(planet is HabitablePlanet habitablePlanet) {
                        habitablePlanets.Add(habitablePlanet);
                    }
                }

                return new ReadOnlyCollection<HabitablePlanet>(habitablePlanets);
            }
        }

        /// <summary>
        /// Возвращает количество планет в системе
        /// </summary>
        public int PlanetsCount { get => this.SystemPlanets.Count; }

        /// <summary>
        /// Возвращает количество колонизируемых планет в системе
        /// </summary>
        public int HabitablePlanetsCount { get => this.SystemHabitablePlanets.Count; }

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
        public long SystemPopulation {
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
        public Resources SystemResources {
            get => this.systemResources;
            private set => this.systemResources = value;
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

        public SystemBuildings Buildings { get => this.buildings; }

        /// <summary>
        ///     Выполняет все операции для перехода на следующий ход
        /// </summary>
        /// <param name="player">
        ///     Игрок, которому принадлежит система
        /// </param>
        public void NextTurn(Player player) {
            this.PlanetsNextTurn(player);
            this.StarsNextTurn();

            this.SystemPopulation = this.SetSystemPopulation();

            this.MineSystemResources(player.OwnedResources);
            this.Buildings.NextTurn(player.OwnedResources);
        }

        private void PlanetsNextTurn(Player player) {
            foreach (Planet planet in this.SystemPlanets) {
                planet.NextTurn(player);
            }
        }

        private void StarsNextTurn() {
            foreach (Star star in this.SystemStars) {
                star.NextTurn();
            }
        }

        private void MineSystemResources(Resources destination) {
            if(this.SystemPopulation == 0) {
                return;
            }

            Miner.Mine(this.MinersCount, this.SystemResources, destination);
        }

        private long SetSystemPopulation() {
            long population = 0;

            foreach (var planet in this.SystemHabitablePlanets) {
                if (planet.IsColonized) {
                    population += planet.Population.Value;
                }
            }

            population += this.Buildings.TotalPopulation;

            return population;
        }

        private byte SetColonizedPlantes() {
            byte colonized = 0;

            foreach (var planet in this.SystemHabitablePlanets) {
                if (planet.IsColonized) {
                    colonized++;
                }
            }

            return colonized;
        }

        private Resources SetResources() {
            double hydrogen = HelperRandomFunctions.GetRandomDouble() * 1E22;
            double commonMetals = HelperRandomFunctions.GetRandomDouble() * 1E24;
            double rareElements = HelperRandomFunctions.GetRandomDouble() * 1E20;

            return new Resources(hydrogen, commonMetals, rareElements);
        }
    }
}
