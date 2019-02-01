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
        /// <summary>
        /// Доля, на которую примерно будет увеличиваться население каждый ход
        /// </summary>
        public const double PopulationGrowthFactor = 0.001;

        private string name;

        public Empire Empire { get; }

        public bool IsDiscoveringNewStarSystems { get; set; } = true;
        public bool IsAutoColonizationEnabled { get; private set; }

        public ColonizationMode ColonizationMode { get; set; } = ColonizationMode.None;
 
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

            this.IsAutoColonizationEnabled =
                new ColonizationModeProcessor(this).DetermineAutoColonizationState(this.ColonizationMode);
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

        /// <summary>
        /// Добавляет в очередь на колонизацию обитаемую планету
        /// </summary>
        /// <param name="planet">
        /// Планета, которая добавляется в очередь
        /// </param>
        public void AddToColonizationQueue(IHabitablePlanet planet) {
            coloniztionQueue.Add(planet);
        }

        /// <summary>
        /// Колонизирует очередь планет, пока имеется достаточно ресурсов
        /// </summary>
        public void TryToColonizeQueue() {
            coloniztionQueue.ColonizeWhilePossible(this.Ships);
        }

        #region Properties
        /// <summary>
        /// Имя игрока
        /// </summary>
        public string Name {
            get => this.name;
            set {
                this.name = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает ресурсы, имеющиеся у игрока
        /// </summary>
        public IMutableResources OwnedResources {
            get => this.Stockpile.PlayerResources;
            //private set => this.Stockpile.PlayerResources = value;
        }

        /// <summary>
        /// Возвращает деньги, имеющиеся у игрока
        /// </summary>
        public double Money {
            get => this.Stockpile.Money;
            private set => this.Stockpile.Money = value;
        }
        #endregion

        /// <summary>
        /// Выполняет все операции по переводу игрока на новый ход
        /// </summary>
        public void NextTurn() {
            this.IsAutoColonizationEnabled =
                new ColonizationModeProcessor(this).DetermineAutoColonizationState(this.ColonizationMode);

            CivilProduction.SustainPopulationNeeds(this.Population, this.OwnedResources);
            new StarSystemsExplorer(this).DiscoverNewSystems();
            this.TryToColonizeQueue();

            this.Empire.NextTurn(this);

            OnStockpileChanged();
        }

        /// <summary>
        /// Добавляет звездную систему игроку
        /// </summary>
        /// <param name="system">
        /// Система, которая добавляется
        /// </param>
        public void AddStarSystem(StarSystem system) {
            this.Empire.AddStarSystem(system);
        }

        /// <summary>
        /// Удаляет звездную систему игрока
        /// </summary>
        /// <param name="system">
        /// Система, которая удаляется
        /// </param>
        public void RemoveStarSystem(StarSystem system) {
            this.Empire.RemoveStarSystem(system);
        }
    }
}
