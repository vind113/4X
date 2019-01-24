using Logic.SpaceObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Logic.PlayerClasses {
    [Serializable]
    public class Empire {
        private const long PerspectiveColonyMinimalPopulation = 10_000_000_000;

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

        public Empire(Player player) {
            this.Container = new StarSystemContainer();
            this.Owner = player;
            this.Hub = new CitizenHub();

            this.AddStarSystem(StarSystemFactory.GetSolarSystem());

            this.SetPopulation();
        }

        public ReadOnlyObservableCollection<StarSystem> StarSystems {
            get => this.Container.StarSystems;
        }

        public void NextTurn(bool isAutoColonizationEnabled, bool isDiscoveringNewStarSystems) {
            this.Container.NextTurn(this.Owner);

            DiscoverNewSystems(isAutoColonizationEnabled, isDiscoveringNewStarSystems);

            this.Hub.SetCitizenHubCapacity(this.Population);
            this.SetPopulation();
        }

        private void DiscoverNewSystems(bool isAutoColonizationEnabled, bool isDiscoveringNewStarSystems) {
            IList<StarSystem> generatedSystems = new List<StarSystem>();
            if (isDiscoveringNewStarSystems) {
                generatedSystems = Discovery.TryToDiscoverNewStarSystem(this.Owner.StarSystemsCount);
            }

            foreach (var system in generatedSystems) {
                this.AddStarSystem(system);
                if (isAutoColonizationEnabled) {
                    ColonizeSystem(system);
                }
            }
        }

        private void ColonizeSystem(StarSystem system) {
            foreach (var planet in system.SystemHabitablePlanets) {
                if (planet.Population.MaxValue >= PerspectiveColonyMinimalPopulation) {
                    if (planet.Colonize(Owner.Ships.GetColonizer()) == ColonizationState.NotColonized) {
                        Owner.AddToColonizationQueue(planet);
                    }
                }
            }
        }

        public void AddStarSystem(StarSystem system) {
            this.Container.AddStarSystem(system);
            system.PropertyChanged += System_PopulationChangedListener;
        }

        public void RemoveStarSystem(StarSystem system) {
            this.Container.RemoveStarSystem(system);
            system.PropertyChanged -= System_PopulationChangedListener;
        }

        private void System_PopulationChangedListener(object sender, PropertyChangedEventArgs e) {
            if (e.PropertyName == nameof(StarSystem.Population)) {
                this.SetPopulation();
            }
        }

        private void SetPopulation() {
            long population = 0;
            foreach (var system in StarSystems) {
                population += system.Population;
            }
            population += this.Hub.CitizensInHub;
            this.Population = population;

            OnPopulationChanged();
        }

        private void OnPopulationChanged() {
            var handler = PopulationChanged;
            handler?.Invoke(this, null);
        }
    }
}
