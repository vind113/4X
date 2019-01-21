using Logic.Resource;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Logic.Buildings {
    [Serializable]
    public class SystemBuildings : INotifyPropertyChanged {
        private ObservableCollection<SpaceBuilding> existing;
        private List<Builder> inConstruction;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public SystemBuildings() {
            this.existing = new ObservableCollection<SpaceBuilding>();
            this.inConstruction = new List<Builder>();
        }

        public void BuildNew(Builder builder) {
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

        public void NextTurn(IMutableResources resources) {
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

        public ReadOnlyCollection<Builder> InConstruction {
            get => new ReadOnlyCollection<Builder>(this.inConstruction);
        }

        //MEMORY LEAK
        // Утечка памяти из-за ObservableCollection(скорее всего)
        // Заменил на for
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

        private void AddCompleted(object sender, BuildingCompletedEventArgs e) {
            if (sender is Builder builder) {
                this.inConstruction.Remove(builder);
                OnPropertyChanged(nameof(SystemBuildings.InConstructionCount));

                this.existing.Add(e.Building);
                OnPropertyChanged(nameof(SystemBuildings.ExistingCount));
            }
        }
    }
}
