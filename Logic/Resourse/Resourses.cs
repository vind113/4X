using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Logic.Resourse {
    public class Resourses : INotifyPropertyChanged {
        private double hydrogen;
        private double commonMetals;
        private double rareEarthElements;

        public Resourses() {
            this.Hydrogen = 0;
            this.CommonMetals = 0;
            this.RareEarthElements = 0;
        }

        public Resourses(double hydrogen, double commonMetals, double rareEarthElements) {
            this.Hydrogen = hydrogen;
            this.CommonMetals = commonMetals;
            this.RareEarthElements = rareEarthElements;
        }

        public static bool operator >=(Resourses res1, Resourses res2) {
            if (res1.Hydrogen >= res2.Hydrogen
             && res1.CommonMetals >= res2.CommonMetals
             && res1.RareEarthElements >= res2.RareEarthElements) {
                return true;
            }

            return false;
        }

        public static bool operator <=(Resourses res1, Resourses res2) {
            if (res1.Hydrogen <= res2.Hydrogen
             && res1.CommonMetals <= res2.CommonMetals
             && res1.RareEarthElements <= res2.RareEarthElements) {
                return true;
            }

            return false;
        }

        public static bool operator >(Resourses res1, Resourses res2) {
            if (res1.Hydrogen > res2.Hydrogen
             && res1.CommonMetals > res2.CommonMetals
             && res1.RareEarthElements > res2.RareEarthElements) {
                return true;
            }

            return false;
        }

        public static bool operator <(Resourses res1, Resourses res2) {
            if (res1.Hydrogen < res2.Hydrogen
             && res1.CommonMetals < res2.CommonMetals
             && res1.RareEarthElements < res2.RareEarthElements) {
                return true;
            }

            return false;
        }

        public static Resourses operator +(Resourses res1, Resourses res2) {
            Resourses result = new Resourses();
            try {
                checked {
                    result.Hydrogen = res1.Hydrogen + res2.Hydrogen;
                    result.CommonMetals = res1.Hydrogen + res2.Hydrogen;
                    result.RareEarthElements = res1.RareEarthElements + res2.RareEarthElements;
                }
            }
            catch(OverflowException) {

            }

            return result;
        }

        public static Resourses operator -(Resourses res1, Resourses res2) {
            try {
                checked {
                    if (res1 < res2) {
                        throw new ArgumentException("First argument can't be less than second");
                    }
                    res1.Hydrogen -= res2.Hydrogen;
                    res1.CommonMetals -= res2.CommonMetals;
                    res1.RareEarthElements -= res2.RareEarthElements;
                }
            }
            catch (OverflowException) {

            }

            return res1;
        }

        public double Hydrogen {
            get => this.hydrogen;
            set {
                if (value >= 0) {
                    this.hydrogen = value;
                    OnPropertyChanged();
                }
            }
        }

        public double CommonMetals {
            get => this.commonMetals;
            set {
                if (value > 0) {
                    this.commonMetals = value;
                    OnPropertyChanged();
                }
            }
        }

        public double RareEarthElements {
            get => this.rareEarthElements;
            set {
                if (value >= 0) {
                    this.rareEarthElements = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
