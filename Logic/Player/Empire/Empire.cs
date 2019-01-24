using Logic.SpaceObjects;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Logic.PlayerClasses {
    [Serializable]
    public class Empire {
        public StarSystemContainer Container { get; }
        public Player Owner { get; }
        public CitizenHub Hub { get; }

        public int StarSystemsCount { get => this.Container.StarSystemsCount; }

        public int OwnedStars { get => this.Container.OwnedStars; }
        public int OwnedPlanets { get => this.Container.OwnedPlanets; }
        public int ColonizedPlanets { get => this.Container.ColonizedPlanets; }

        public long Population { get; private set; }

        [field: NonSerialized]
        public event EventHandler PopulationChanged;

        [field: NonSerialized]
        public event EventHandler ColonizedCountChanged;

        public Empire(Player player) {
            this.Container = new StarSystemContainer();
            this.Owner = player;
            this.Hub = new CitizenHub();

            this.AddStarSystem(StarSystemFactory.GetSolarSystem());

            this.SetTotalPopulation();
        }

        public ReadOnlyObservableCollection<StarSystem> StarSystems {
            get => this.Container.StarSystems;
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
            this.Container.AddStarSystem(system);
            system.PropertyChanged += System_PropertyChanged;
        }

        public void RemoveStarSystem(StarSystem system) {
            this.Container.RemoveStarSystem(system);
            system.PropertyChanged -= System_PropertyChanged;
        }

        private void System_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(StarSystem.ColonizedCount)) {
                this.Container.SetColonized();
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

        private void OnColonizedCountChanged() {
            var handler = ColonizedCountChanged;
            handler?.Invoke(this, null);
        }
    }
}
