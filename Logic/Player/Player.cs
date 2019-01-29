using System;
using System.Collections.ObjectModel;
using Logic.Resource;
using Logic.SpaceObjects;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Logic.GameClasses;

namespace Logic.PlayerClasses {
    [Serializable]
    public class Player : INotifyPropertyChanged, IPlayer {
        public const double PopulationGrowthFactor = 0.001;

        private string name;

        public Empire Empire { get; }

        public bool IsDiscoveringNewStarSystems { get; set; } = true;
        public bool IsAutoColonizationEnabled { get; set; } = true;

        public Stockpile Stockpile { get; }

        public CitizenHub Hub { get => this.Empire.Hub; }
        public IShipsFactory Ships { get; }

        private ColoniztionQueue coloniztionQueue = new ColoniztionQueue();

        public int OwnedStars { get => this.Empire.OwnedStars; }
        public int OwnedPlanets { get => this.Empire.OwnedPlanets; }

        public int StarSystemsCount { get => this.Empire.StarSystemsCount; }
        public int ColonizedPlanets { get => this.Empire.ColonizedPlanets; }

        public long Population { get => this.Empire.Population; }
        public ReadOnlyObservableCollection<StarSystem> StarSystems { get => this.Empire.StarSystems; }

        public Player() {
            this.Stockpile = new Stockpile();
            this.Stockpile.PlayerResources.Add(new Resources(1_000_000_000, 10_000_000_000, 10_000_000));

            this.Empire = new Empire();

            this.Ships = new ShipsFactory(this.OwnedResources);
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
            //private set => this.Stockpile.PlayerResources = value;
        }

        public double Money {
            get => this.Stockpile.Money;
            private set => this.Stockpile.Money = value;
        }
        #endregion

        public void NextTurn() {
            CivilProduction.SustainPopulationNeeds(this.Population, this.OwnedResources);
            new StarSystemsExplorer(this).DiscoverNewSystems();
            this.TryToColonizeQueue();

            this.Empire.NextTurn(this);

            OnStockpileChanged();
        }

        public void AddStarSystem(StarSystem system) {
            this.Empire.AddStarSystem(system);
        }

        public void RemoveStarSystem(StarSystem system) {
            this.Empire.RemoveStarSystem(system);
        }
    }
}
