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
    [Serializable]
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

        private double populationGrowthFactor = 0.001;

        public Player() {
            this.Stockpile = new Stockpile();
            this.Stockpile.PlayerResourses.Add(new Resourses(1_000_000_000, 10_000_000_000, 10_000_000));

            this.hub = new CitizenHub();

            this.starSystems = new ObservableCollection<StarSystem>();

            this.AddStarSystem(StarSystemFactory.GetSolarSystem());

            this.planetsToColonize = new Queue<Planet>();

            this.Ships = new Ships();
        }

        [field: NonSerialized]
        public event EventHandler<StockpileChangedEventArgs> StockpileChanged;

        [field: NonSerialized]
        public event EventHandler<PopulationChangedEventArgs> PopulationChanged;

        [field: NonSerialized]
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

            if (!this.planetsToColonize.Contains(planet)) {
                this.planetsToColonize.Enqueue(planet);
            }
        }

        public void TryToColonizeQueue() {
            if(this.planetsToColonize.Count == 0) {
                return;
            }

            while (this.planetsToColonize.Count > 0) {
                ColonizationState state =
                    this.planetsToColonize.Peek().Colonize(this);

                if (state == ColonizationState.NotColonized) {
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

        public CitizenHub Hub {
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

        public int ToColonize {
            get => this.planetsToColonize.Count;
        }

        public double TotalPopulation {
            get {
                double population = 0;
                foreach(var system in StarSystems) {
                    foreach(var planet in system.SystemPlanets) {
                        population += planet.Population;
                    }
                }
                population += this.Hub.CitizensInHub;
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

        public double PopulationGrowthFactor {
            get => this.populationGrowthFactor;
            set {
                if (value >= 0 && value != this.populationGrowthFactor) {
                    this.populationGrowthFactor = value;
                    OnPropertyChanged();
                }
            }
        }

        public Ships Ships {
            get => this.ships;
            private set => this.ships = value;
        }
        #endregion

        public void NextTurn(bool isAutoColonizationEnabled, bool isDiscoveringNewStarSystems) {
            foreach (StarSystem system in this.StarSystems) {
                system.NextTurn(this);
            }

            Goods.SustainPopulationNeeds(this);
            this.TryToColonizeQueue();

            if (isDiscoveringNewStarSystems) {
                Discovery.NewStarSystem(isAutoColonizationEnabled, this);
            }

            SetCitizenHubCapacity();

            OnStockpileChanged();
            OnPopulationChanged();
        }

        private void SetCitizenHubCapacity() {
            double newHubCapacity = Math.Floor(this.TotalPopulation / 1000);

            if (newHubCapacity > this.Hub.CitizensInHub) {
                this.Hub.MaximumCount = newHubCapacity;
            }
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
    }
}
