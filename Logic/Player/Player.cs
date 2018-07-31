using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Logic.Resourse;
using Logic.SpaceObjects;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Logic.GameClasses;
using Logic.SupportClasses;

namespace Logic.PlayerClasses {
    public class Player : INotifyPropertyChanged {
        private string name;

        private Stockpile stockpile;
        private CitizenHub hub;
        private ObservableCollection<StarSystem> starSystems;

        private Queue<Planet> planetsToColonize;

        private Ships ships;

        private int ownedStars;
        private int colonizedPlanets;
        private int ownedPlanets;

        public Player() {
            this.Stockpile = new Stockpile();
            this.hub = new CitizenHub();

            this.starSystems = new ObservableCollection<StarSystem>();

            this.AddStarSystem(StarSystemFactory.GetSolarSystem());

            this.planetsToColonize = new Queue<Planet>();
            this.ships = new Ships();
        }

        public Player(Stockpile stockpile, IEnumerable<StarSystem> starSystems) {
            this.stockpile = stockpile ?? throw new ArgumentNullException(nameof(stockpile));
            this.starSystems = new ObservableCollection<StarSystem>(starSystems) ?? throw new ArgumentNullException(nameof(starSystems));

            this.hub = new CitizenHub();
            this.planetsToColonize = new Queue<Planet>();
            this.ships = new Ships();
        }

        public event EventHandler<StockpileChangedEventArgs> StockpileChanged;
        public event EventHandler<PopulationChangedEventArgs> PopulationChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnStockpileChanged() {
            var handler = StockpileChanged;
            handler?.Invoke(this, new StockpileChangedEventArgs(this.Money, this.OwnedResourses));
        }

        private void OnPopulationChanged() {
            var handler = PopulationChanged;
            handler?.Invoke(this, new PopulationChangedEventArgs(this.TotalPopulation));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddToColonizationQueue(Planet planet) {
            if (planet == null) {
                throw new ArgumentNullException(nameof(planet));
            }

            if (planet.MaximumPopulation == 0) {
                return;
            }

            this.planetsToColonize.Enqueue(planet);
        }

        public void TryToColonizeQueue() {
            if(this.planetsToColonize.Count == 0) {
                return;
            }

            while (this.planetsToColonize.Count > 0) {
                bool colonized = this.planetsToColonize.Peek().Colonize(this);
                if (!colonized) {
                    break;
                }
                this.planetsToColonize.Dequeue();
            }
        }

        #region Properties
        public string Name {
            get => this.name;
            set {
                this.name = value;
                OnPropertyChanged();
            }
        }

        public CitizenHub PlayerCitizenHub {
            get => this.hub;
            private set => this.hub = value;
        }

        public Resourses OwnedResourses {
            get => this.stockpile.PlayerResourses;
            set => this.stockpile.PlayerResourses = value;
        }

        public double Money {
            get => this.stockpile.Money;
            private set => this.stockpile.Money = value;
        }

        public ReadOnlyObservableCollection<StarSystem> StarSystems {
            get => new ReadOnlyObservableCollection<StarSystem>(this.starSystems);
        }

        public int StarSystemsCount {
            get => this.starSystems.Count;
        }

        public double TotalPopulation {
            get {
                double population = 0;
                foreach(var system in StarSystems) {
                    foreach(var planet in system.SystemPlanets) {
                        population += planet.Population;
                    }
                }
                population += this.PlayerCitizenHub.CitizensInHub;
                return population;
            }
        }

        public int ColonizedPlanets {
            get => this.colonizedPlanets;
            private set {
                this.colonizedPlanets = value;
                OnPropertyChanged();
            }
        }

        public int OwnedPlanets {
            get => this.ownedPlanets;
            private set {
                this.ownedPlanets = value;
                OnPropertyChanged();
            }
        }

        public int OwnedStars {
            get => this.ownedStars;
            private set {
                this.ownedStars = value;
                OnPropertyChanged();
            }
        }

        public Stockpile Stockpile {
            get => this.stockpile;
            private set {
                this.stockpile = value;
                OnPropertyChanged();
            }
        }

        public Ships Ships {
            get => this.ships;
            private set {
                this.ships = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public void NextTurn(bool isAutoColonizationEnabled, bool isDiscoveringNewStarSystems) {
            if (isAutoColonizationEnabled) {
                this.TryToColonizeQueue();
            }

            foreach (StarSystem system in this.StarSystems) {
                system.NextTurn(this);
            }

            if (isDiscoveringNewStarSystems) {
                DiscoverNewStarSystem(isAutoColonizationEnabled);
            }

            SetCitizenHubCapacity();

            OnStockpileChanged();
            OnPopulationChanged();
        }

        private void SetCitizenHubCapacity() {
            double newHubCapacity = Math.Floor(this.TotalPopulation / 1000);

            if (newHubCapacity > this.PlayerCitizenHub.CitizensInHub) {
                this.PlayerCitizenHub.MaximumCount = newHubCapacity;
            }
        }

        public bool GetColonizer() {
            return this.Ships.GetColonizer(this);
        }

        public void AddStarSystem(StarSystem system) {
            if (system == null) {
                throw new ArgumentNullException(nameof(system));
            }

            this.starSystems.Add(system);
            this.AddBodiesCount(system);
            system.PropertyChanged += System_PropertyChanged;
        }

        private void System_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if(e.PropertyName == nameof(StarSystem.ColonizedCount)) {
                SetColonized();
            }
        }

        private void SetColonized() {
            int colonized = 0;

            foreach (var system in this.StarSystems) {
                foreach (var planet in system.SystemPlanets) {
                    if (planet.IsColonized) {
                        colonized++;
                    }
                }
            }

            this.ColonizedPlanets = colonized;
        }

        private void AddBodiesCount(StarSystem system) {
            this.OwnedPlanets += system.PlanetsCount;
            this.OwnedStars += system.StarsCount;
            this.ColonizedPlanets += system.ColonizedCount;
        }

        public void RemoveStarSystem(StarSystem system) {
            if (system == null) {
                throw new ArgumentNullException(nameof(system));
            }

            if (this.starSystems.Contains(system)) {
                this.starSystems.Remove(system);
            }
        }

        private void DiscoverNewStarSystem(bool isAutoColonizationEnabled) {
            //с такой вероятностью каждый ход будет открываться новая система
            //возможно добавить зависимость от уровня технологий
            //оптимальное значение - 0.15
            double discoveryProbability = 0.15;

            if (HelperRandomFunctions.ProbableBool(discoveryProbability)) {
                int maxSystemsToGenerate = 0;
                int systemsToGenerate = 0;
                StarSystem generatedSystem = null;

                checked {
                    maxSystemsToGenerate = (int)((Math.Sqrt(this.StarSystemsCount)) / 2);
                    systemsToGenerate = HelperRandomFunctions.GetRandomInt(1, maxSystemsToGenerate + 1);
                }

                for (int index = 0; index < systemsToGenerate; index++) {
                    generatedSystem = StarSystemFactory.GetStarSystem($"System {this.StarSystemsCount + 1} #{index}");

                    if (isAutoColonizationEnabled) {
                        foreach (var planet in generatedSystem.SystemPlanets) {
                            planet.Colonize(this);
                        }
                    }

                    this.AddStarSystem(generatedSystem);
                }
            }
        }
    }
}
