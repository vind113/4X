using Logic.SpaceObjects;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Logic.PlayerClasses {
    [Serializable]
    public class Empire {
        public Player Owner { get; }
        public CitizenHub Hub { get; }

        private ObservableCollection<StarSystem> starSystems;
        public int StarSystemsCount { get => starSystems.Count; }

        public int OwnedStars { get; private set; }
        public int OwnedPlanets { get; private set; }
        public int ColonizedPlanets { get; private set; }

        public long Population { get; private set; }

        [field: NonSerialized]
        public event EventHandler PopulationChanged;

        [field: NonSerialized]
        public event EventHandler BodiesCountChanged;

        [field: NonSerialized]
        public event EventHandler ColonizedCountChanged;

        public Empire(Player player) {
            this.Owner = player;
            starSystems = new ObservableCollection<StarSystem>();
            this.Hub = new CitizenHub();

            this.AddStarSystem(StarSystemFactory.GetSolarSystem());

            this.SetTotalPopulation();
        }

        public ReadOnlyObservableCollection<StarSystem> StarSystems {
            get => new ReadOnlyObservableCollection<StarSystem>(this.starSystems);
        }

        public void NextTurn(bool isAutoColonizationEnabled, bool isDiscoveringNewStarSystems) {
            foreach (StarSystem system in this.StarSystems) {
                system.NextTurn(this.Owner);
            }

            if (isDiscoveringNewStarSystems) {
                Discovery.TryToDiscoverNewStarSystem(isAutoColonizationEnabled, this.Owner);
            }

            this.Hub.SetCitizenHubCapacity(this.Population);
            this.SetTotalPopulation();
        }

        public void AddStarSystem(StarSystem system) {
            if (system == null) {
                throw new ArgumentNullException(nameof(system));
            }

            this.starSystems.Add(system);
            this.AddBodiesCount(system);
            system.PropertyChanged += System_PropertyChanged;
            OnBodiesCountChanged();
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
                SubtractBodiesCount(system);
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

        private void System_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(StarSystem.ColonizedCount)) {
                this.SetColonized();
                OnColonizedCountChanged();
            }
            else if (e.PropertyName == nameof(StarSystem.Population)) {
                this.SetTotalPopulation();
            }
        }

        private void SetTotalPopulation() {
            long population = 0;
            foreach (var system in StarSystems) {
                population += system.Population;
            }
            population += this.Hub.CitizensInHub;
            this.Population = population;

            UpdatePopulation();
        }

        private void UpdatePopulation() {
            OnPopulationChanged();
        }

        private void OnPopulationChanged() {
            var handler = PopulationChanged;
            handler?.Invoke(this, null);
        }

        private void OnBodiesCountChanged() {
            var handler = BodiesCountChanged;
            handler?.Invoke(this, null);
        }

        private void OnColonizedCountChanged() {
            var handler = ColonizedCountChanged;
            handler?.Invoke(this, null);
        }
    }
}
