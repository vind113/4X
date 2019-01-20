using System.ComponentModel;
using Logic.Resource;
using System.Runtime.CompilerServices;
using System;
using Logic.SupportClasses;

namespace Logic.SpaceObjects {

    [Serializable]
    public abstract class CelestialBody : INotifyPropertyChanged {
        private string name;

        public double Area { get; }
        public double Radius { get; }
        public IMutableObservableResources BodyResource { get; protected set; }

        public CelestialBody(string name, double radius) {
            this.name = name;
            this.Radius = radius;
            this.Area = Math.Floor(HelperMathFunctions.SphereArea(this.Radius));
        }

        public string Name {
            get => this.name;
            set {
                this.name = value;
                OnPropertyChanged();
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
