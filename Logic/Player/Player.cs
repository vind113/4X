using System;
using System.Collections.ObjectModel;
using Logic.Resource;
using Logic.SpaceObjects;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Logic.GameClasses;

namespace Logic.PlayerClasses {
    [Serializable]
    public class Player : INotifyPropertyChanged, IPlayer
    {
        public const double PopulationGrowthFactor = 0.001;

        private string name;

        private Stockpile stockpile;
        private CitizenHub hub;
        private ObservableCollection<StarSystem> starSystems;

        private ColoniztionQueue coloniztionQueue = new ColoniztionQueue();

        private IShipsFactory ships;

        private int ownedStars;
        private int colonizedPlanets;
        private int ownedPlanets;

        public Player() {
            this.Stockpile = new Stockpile();
            this.Stockpile.PlayerResources.Add(new Resources(1_000_000_000, 10_000_000_000, 10_000_000));

            this.hub = new CitizenHub();

            this.starSystems = new ObservableCollection<StarSystem>();

            this.AddStarSystem(StarSystemFactory.GetSolarSystem());

            this.Ships = new ShipsFactory(this.OwnedResources);
        }

        [field: NonSerialized]
        public event EventHandler<StockpileChangedEventArgs> StockpileChanged;

        [field: NonSerialized]
        public event EventHandler<PopulationChangedEventArgs> PopulationChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnStockpileChanged() {
            var handler = StockpileChanged;
            handler?.Invoke(this, new StockpileChangedEventArgs(this.Money, this.OwnedResources));
        }

        private void OnPopulationChanged() {
            var handler = PopulationChanged;
            handler?.Invoke(this, new PopulationChangedEventArgs(this.TotalPopulation));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddToColonizationQueue(HabitablePlanet planet) {
            coloniztionQueue.Add(planet);
        }

        public void TryToColonizeQueue() {
            coloniztionQueue.ColonizeWhilePossible(this.Ships);
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

        public IMutableResources OwnedResources {
            get => this.stockpile.PlayerResources;
            set => this.stockpile.PlayerResources = value;
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

        public long TotalPopulation {
            get {
                long population = 0;
                foreach(var system in StarSystems) {
                    foreach(var planet in system.SystemHabitablePlanets) {
                        population += planet.Population.Value;
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

        public IShipsFactory Ships {
            get => this.ships;
            private set => this.ships = value;
        }
        #endregion

        public void NextTurn(bool isAutoColonizationEnabled, bool isDiscoveringNewStarSystems) {
            foreach (StarSystem system in this.StarSystems) {
                system.NextTurn(this);
            }

            CivilProduction.SustainPopulationNeeds(this.TotalPopulation, this.OwnedResources);
            this.TryToColonizeQueue();

            if (isDiscoveringNewStarSystems) {
                Discovery.TryToDiscoverNewStarSystem(isAutoColonizationEnabled, this);
            }

            SetCitizenHubCapacity();

            OnStockpileChanged();
            OnPopulationChanged();
        }

        private void SetCitizenHubCapacity() {
            long newHubCapacity = this.TotalPopulation / 1000;

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

        private void AddBodiesCount(StarSystem system) {
            this.OwnedPlanets += system.PlanetsCount;
            this.OwnedStars += system.StarsCount;
            this.ColonizedPlanets += system.ColonizedCount;
        }

        private void System_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if(e.PropertyName == nameof(StarSystem.ColonizedCount)) {
                SetColonized();
            }
        }

        private void SetColonized() {
            int colonized = 0;

            foreach (var system in this.StarSystems) {
                foreach (var planet in system.SystemHabitablePlanets) {
                    if (planet.IsColonized) {
                        colonized++;
                    }
                }
            }

            this.ColonizedPlanets = colonized;
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
