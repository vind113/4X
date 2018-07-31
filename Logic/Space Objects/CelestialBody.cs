using System.ComponentModel;
using Logic.Resourse;
using System.Runtime.CompilerServices;

namespace Logic.SpaceObjects {

    public abstract class CelestialBody : INotifyPropertyChanged {
        protected string name;                //имя небесного тела
        protected double area;      //площадь небесного тела
        protected double radius;    //радиус небесного тела
        private Resourses bodyResourse;   //ресурсы на небесном теле

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
