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

        private ObservableCollection<StarSystem> starSystems;

        public Stockpile Stockpile { get; }

        public CitizenHub Hub { get; }
        public IShipsFactory Ships { get; }

        private ColoniztionQueue coloniztionQueue = new ColoniztionQueue();
        private int ownedStars;
        private int colonizedPlanets;
        private int ownedPlanets;

        public long TotalPopulation { get; private set; } = 0;

        public Player() {
            this.Stockpile = new Stockpile();
            this.Stockpile.PlayerResources.Add(new Resources(1_000_000_000, 10_000_000_000, 10_000_000));

            this.Hub = new CitizenHub();

            this.starSystems = new ObservableCollection<StarSystem>();

            this.AddStarSystem(StarSystemFactory.GetSolarSystem());

            this.Ships = new ShipsFactory(this.OwnedResources);

            this.SetTotalPopulation();
        }

        [field: NonSerialized]
        public event EventHandler<StockpileChangedEventArgs> StockpileChanged;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnStockpileChanged() {
            var handler = StockpileChanged;
            handler?.Invoke(this, new StockpileChangedEventArgs(this.Money, this.OwnedResources));
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

        public IMutableResources OwnedResources {
            get => this.Stockpile.PlayerResources;
            private set => this.Stockpile.PlayerResources = value;
        }

        public double Money {
            get => this.Stockpile.Money;
            private set => this.Stockpile.Money = value;
        }

        public ReadOnlyObservableCollection<StarSystem> StarSystems {
            get => new ReadOnlyObservableCollection<StarSystem>(this.starSystems);
        }

        public int StarSystemsCount {
            get => this.starSystems.Count;
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

            this.Hub.SetCitizenHubCapacity(this.TotalPopulation);

            OnStockpileChanged();
            this.SetTotalPopulation();
        }

        public void AddStarSystem(StarSystem system) {
            if (system == null) {
                throw new ArgumentNullException(nameof(system));
            }

            this.starSystems.Add(system);
            this.AddBodiesCount(system);
            system.PropertyChanged += System_PropertyChanged;
        }

        public void RemoveStarSystem(StarSystem system) {
            if (system == null) {
                throw new ArgumentNullException(nameof(system));
            }

            if (this.starSystems.Contains(system)) {
                this.starSystems.Remove(system);
            }
        }

        private void AddBodiesCount(StarSystem system) {
            this.OwnedPlanets += system.PlanetsCount;
            this.OwnedStars += system.StarsCount;
            this.ColonizedPlanets += system.ColonizedCount;
        }

        private void System_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(StarSystem.ColonizedCount)) {
                this.SetColonized();
            }
            else if (e.PropertyName == nameof(StarSystem.SystemPopulation)) {
                this.SetTotalPopulation();
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

        public void SetTotalPopulation() {
            long population = 0;
            foreach (var system in StarSystems) {
                population += system.SystemPopulation;
            }
            population += this.Hub.CitizensInHub;
            this.TotalPopulation = population;
            OnPropertyChanged(nameof(Player.TotalPopulation));
        }
    }
}
