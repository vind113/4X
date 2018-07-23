using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Logic.Resourse {
    public class Resourses : INotifyPropertyChanged {
        private double hydrogen;
        private double commonMetals;
        private double rareEarthElements;

        public Resourses(double hydrogen, double commonMetals, double rareEarthElements) {
            this.hydrogen = hydrogen;
            this.commonMetals = commonMetals;
            this.rareEarthElements = rareEarthElements;
        }

        public double Hydrogen {
            get => this.hydrogen;
            set {
                this.hydrogen = value;
                OnPropertyChanged();
            }
        }

        public double CommonMetals {
            get => this.commonMetals;
            set {
                this.commonMetals = value;
                OnPropertyChanged();
            }
        }

        public double RareEarthElements {
            get => this.rareEarthElements;
            set {
                this.rareEarthElements = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
