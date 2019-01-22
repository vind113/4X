using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Logic.PlayerClasses;
using Logic.Resource;
using Logic.SupportClasses;
using Logic.Buildings;
using Logic.PopulationClasses;

namespace Logic.SpaceObjects {
    /// <summary>
    ///     Представляет звездную систему
    /// </summary>
    [Serializable]
    public class StarSystem : INotifyPropertyChanged {
        private string name;

        private readonly List<Star> systemStars;
        private readonly List<Planet> systemPlanets;
        private MinerFleet systemMiners;

        public SystemBuildings Buildings { get; }

        /// <summary>
        /// Возвращает ресурсы системы
        /// </summary>
        public IMutableResources SystemResources { get; private set; }

        private byte colonizedCount;
        private long population;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

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

            this.Buildings = new SystemBuildings();

            this.systemMiners = new MinerFleet();
           
            foreach (var planet in this.SystemPlanets) {
                planet.PropertyChanged += this.Planet_PropertyChanged;
            }

            this.SetSystemPopulation();

            this.ColonizedCount = this.SetColonizedPlantes();

            SetMiners();
            this.SystemResources = new StarSystemResourceGenerator().GenerateResources();
        }

        /// <summary>
        /// Возвращает ссылку на коллекцию объектов <see cref="Star"/> системы
        /// </summary>
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
        public long Population {
            get => population;
            private set {
                if (this.population != value) {
                    this.population = value;
                }
            }
        }

        /// <summary>
        /// Возвращает количество добывающих кораблей системы
        /// </summary>
        public int MinersCount {
            get => this.systemMiners.MinersCount;
        }

        //TODO: переработать этот костыль
        public void SetMiners() {
            if (this.Population > 0) {
                int minersToAdd = 50;
                this.systemMiners = new MinerFleet(minersToAdd);
            }
        }

        /// <summary>
        ///     Выполняет все операции для перехода на следующий ход
        /// </summary>
        /// <param name="player">
        ///     Игрок, которому принадлежит система
        /// </param>
        public void NextTurn(Player player) {
            this.PlanetsNextTurn(player);
            this.StarsNextTurn();

            this.systemMiners.Mine(this.SystemResources, player.OwnedResources);
            this.Buildings.NextTurn(player.OwnedResources);

            this.SetSystemPopulation();
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

        private void SetSystemPopulation() {
            long population = 0;

            foreach (var planet in this.SystemHabitablePlanets) {
                population += planet.PopulationValue;
            }

            population += this.Buildings.Population;
            this.Population = population;
        }

        public void UpdatePopulation() {
            OnPropertyChanged(nameof(StarSystem.Population));
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Planet_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (sender is HabitablePlanet planet) {
                if (e.PropertyName == nameof(HabitablePlanet.IsColonized)) {
                    this.ColonizedCount += (byte)((planet.IsColonized) ? 1 : -1);
                }
                else if (e.PropertyName == nameof(HabitablePlanet.Population.Value)) {
                    this.SetSystemPopulation();
                }
            }
        }
    }
}
