using System.ComponentModel;
using Logic.Resourse;
using System.Runtime.CompilerServices;
using System;
using Logic.SupportClasses;

namespace Logic.SpaceObjects {

    [Serializable]
    public abstract class CelestialBody : INotifyPropertyChanged {
        private string name;                
        private double area;      
        private double radius;    
        private Resourses bodyResourse;

        public CelestialBody(string name, double radius) {
            this.name = name;
            this.radius = radius;
            this.area = Math.Floor(HelperMathFunctions.SphereArea(this.radius));
        }

        public string Name {
            get => this.name;
            set {
                this.name = value;
                OnPropertyChanged();
            }
        }
        public double Area { get => this.area; }

        public double Radius { get => this.radius; }

        public Resourses BodyResourse {
            get => this.bodyResourse;
            protected set => this.bodyResourse = value;
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
