using System.ComponentModel;

namespace _4XGame.ViewModel {
    public class BodiesCount : INotifyPropertyChanged {
        public int Stars { get; private set; }
        public int Planets { get; private set; }
        public int Systems { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public BodiesCount() {
            this.Stars = 0;
            this.Planets = 0;
            this.Systems = 0;
        }

        public void SetBodiesCount(int systems, int stars, int planets) {
            this.Systems = systems;
            OnPropertyChanged(nameof(BodiesCount.Systems));

            this.Stars = stars;
            OnPropertyChanged(nameof(BodiesCount.Stars));

            this.Planets = planets;
            OnPropertyChanged(nameof(BodiesCount.Planets));
        }

        private void OnPropertyChanged(string propertyName) {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


