using Logic.SpaceObjects;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Logic.PlayerClasses {
    [Serializable]
    public class StarSystemContainer {
        private ObservableCollection<StarSystem> starSystems;

        /// <summary>
        /// Возвращает количество звездных систем в контейнере
        /// </summary>
        public int StarSystemsCount { get => this.StarSystems.Count; }

        /// <summary>
        /// Возвращает общее количество звезд всех систем в контейнере
        /// </summary>
        public int OwnedStars { get; private set; }

        /// <summary>
        /// Возвращает общее количество планет всех систем в контейнере
        /// </summary>
        public int OwnedPlanets { get; private set; }

        /// <summary>
        /// Возвращает общее количество колонизированых планет всех систем в контейнере
        /// </summary>
        public int ColonizedPlanets { get; private set; }


        [field: NonSerialized]
        public event EventHandler BodiesCountChanged;

        [field: NonSerialized]
        public event EventHandler ColonizedCountChanged;

        public StarSystemContainer() {
            starSystems = new ObservableCollection<StarSystem>();
        }

        public ReadOnlyObservableCollection<StarSystem> StarSystems {
            get => new ReadOnlyObservableCollection<StarSystem>(this.starSystems);
        }

        public void NextTurn(Player player) {
            foreach (StarSystem system in this.StarSystems) {
                system.NextTurn(player);
            }
        }

        public void AddStarSystem(StarSystem system) {
            if (system == null) {
                throw new ArgumentNullException(nameof(system));
            }

            if (!this.StarSystems.Contains(system)) {
                this.starSystems.Add(system);
                this.AddBodiesCount(system);
                system.PropertyChanged += System_ColonizedCountChangedListener;
                OnBodiesCountChanged();
            }
        }

        private void AddBodiesCount(StarSystem system) {
            this.OwnedPlanets += system.PlanetsCount;
            this.OwnedStars += system.StarsCount;
            this.ColonizedPlanets += system.ColonizedCount;
        }

        private void System_ColonizedCountChangedListener(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(StarSystem.ColonizedCount)) {
                SetColonized();
                OnColonizedCountChanged();
            }
        }

        private void OnBodiesCountChanged() {
            var handler = BodiesCountChanged;
            handler?.Invoke(this, null);
        }

        public void RemoveStarSystem(StarSystem system) {
            if (system == null) {
                throw new ArgumentNullException(nameof(system));
            }

            if (this.starSystems.Contains(system)) {
                this.starSystems.Remove(system);
                SubtractBodiesCount(system);
                system.PropertyChanged -= System_ColonizedCountChangedListener;
                OnBodiesCountChanged();
            }
        }

        private void SubtractBodiesCount(StarSystem system) {
            this.OwnedPlanets -= system.PlanetsCount;
            this.OwnedStars -= system.StarsCount;
            this.ColonizedPlanets -= system.ColonizedCount;
        }

        private void SetColonized() {
            int colonized = 0;

            foreach (var system in this.StarSystems) {
                colonized += system.ColonizedCount;
            }

            this.ColonizedPlanets = colonized;
        }

        private void OnColonizedCountChanged() {
            var handler = ColonizedCountChanged;
            handler?.Invoke(this, null);
        }
    }
}
