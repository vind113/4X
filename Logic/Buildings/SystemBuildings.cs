﻿using Logic.Resource;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Logic.Buildings {
    public enum Buildings : byte {
        None, Habitat, Ringworld
    }

    [Serializable]
    public class SystemBuildings : INotifyPropertyChanged {
        private ObservableCollection<SpaceBuilding> existing;
        private List<HabitatBuilder> inConstruction;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public SystemBuildings() {
            this.existing = new ObservableCollection<SpaceBuilding>();
            this.inConstruction = new List<HabitatBuilder>();
        }

        public void BuildNew(HabitatBuilder builder) {
            if (builder == null) {
                throw new ArgumentNullException(nameof(builder));
            }

            if (this.inConstruction.Contains(builder)) {
                return;
            }

            this.inConstruction.Add(builder);
            OnPropertyChanged(nameof(SystemBuildings.InConstructionCount));

            builder.Completed += AddCompleted;
        }

        public void NextTurn(Resources resources) {
            if (inConstruction.Count <= 0) {
                return;
            }

            foreach (var building in inConstruction.ToArray()) {
                building.OneTurnProgress(resources);
            }
        }

        #region Properties
        public int ExistingCount { get => this.existing.Count; }
        public int InConstructionCount { get => this.inConstruction.Count; }

        public ReadOnlyObservableCollection<SpaceBuilding> Existing {
            get => new ReadOnlyObservableCollection<SpaceBuilding>(this.existing);
        }

        public ReadOnlyCollection<HabitatBuilder> InConstruction {
            get => new ReadOnlyCollection<HabitatBuilder>(this.inConstruction);
        }

        //MEMORY LEAK
        //UPD: Fixed but I do not know how(smth related to foreach)
        //UPD2: Try naive foreach implementation
        public long TotalPopulation {
            get {
                long population = 0;
                /*foreach (var habitat in this.Existing) {
                    //population += habitat.Population;
                }*/
                for (int i = 0; i < this.ExistingCount; i++) {
                    population += this.Existing[i].Population.Value;
                }
                return population;
            }
        }
        #endregion

        private void OnPropertyChanged(string propertyName) {
            var handler = this.PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddCompleted(object sender, SpaceBuildingCompletedEventArgs e) {
            if (sender is HabitatBuilder builder) {
                this.inConstruction.Remove(builder);
                OnPropertyChanged(nameof(SystemBuildings.InConstructionCount));

                this.existing.Add(e.Habitat);
                OnPropertyChanged(nameof(SystemBuildings.ExistingCount));
            }
        }
    }
}
