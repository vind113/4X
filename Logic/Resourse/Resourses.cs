using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Logic.Resourse {
    /// <summary>
    /// Представляет ресурсы
    /// </summary>
    [Serializable]
    public class Resourses : INotifyPropertyChanged {
        private double hydrogen;
        private double commonMetals;
        private double rareEarthElements;

        private static readonly Resourses zero = new Resourses();

        /// <summary>
        /// Инициализирует новый объект <see cref="Resourses"/> значениями по умолчанию
        /// </summary>
        public Resourses() {
            this.Hydrogen = 0;
            this.CommonMetals = 0;
            this.RareEarthElements = 0;
        }

        /// <summary>
        /// Инициализирует новый объект <see cref="Resourses"/> заданными значениями
        /// </summary>
        /// <param name="hydrogen">
        /// Количество энергетических ресурсов
        /// </param>
        /// <param name="commonMetals">
        /// Количество обычных металлов
        /// </param>
        /// <param name="rareEarthElements">
        /// Количество редких элементов
        /// </param>
        public Resourses(double hydrogen, double commonMetals, double rareEarthElements) {
            this.Hydrogen = hydrogen;
            this.CommonMetals = commonMetals;
            this.RareEarthElements = rareEarthElements;
        }

        /// <summary>
        /// Сравнивает два объекта <see cref="Resourses"/>
        /// </summary>
        /// <returns>Булевое значение, показывающее, равны ли соответствующие составные объектов <see cref="Resourses"/></returns>
        public static bool AreEqual(Resourses res1, Resourses res2) {
            if (res1.Hydrogen == res2.Hydrogen
             && res1.CommonMetals == res2.CommonMetals
             && res1.RareEarthElements == res2.RareEarthElements) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Сравнивает два объекта <see cref="Resourses"/>
        /// </summary>
        /// <returns>Булевое значение, показывающее, отличаются ли соответствующие составные объектов <see cref="Resourses"/></returns>
        public static bool AreNotEqual(Resourses res1, Resourses res2) {
            if (res1.Hydrogen != res2.Hydrogen
             || res1.CommonMetals != res2.CommonMetals
             || res1.RareEarthElements != res2.RareEarthElements) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Прибавляет ресурсы переданного объекта <see cref="Resourses"/>
        /// </summary>
        public void Add(Resourses parameter) {
            try {
                checked {
                    this.Hydrogen += parameter.Hydrogen;
                    this.CommonMetals += parameter.CommonMetals;
                    this.RareEarthElements += parameter.RareEarthElements;
                }
            }
            catch(OverflowException) {

            }
        }

        /// <summary>
        /// Вычитает ресурсы переданного объекта <see cref="Resourses"/>
        /// </summary>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="OverflowException"/>
        public Resourses Substract(Resourses parameter) {
            try {
                checked {
                    if (!this.CanSubstract(parameter)) {
                        throw new ArgumentException("Argument can't be greater than parameter");
                    }
                    this.Hydrogen -= parameter.Hydrogen;
                    this.CommonMetals -= parameter.CommonMetals;
                    this.RareEarthElements -= parameter.RareEarthElements;
                }
            }
            catch (OverflowException) {

            }

            return this;
        }

        public bool CanSubstract(Resourses res) {
            if (this.Hydrogen >= res.Hydrogen
             && this.CommonMetals >= res.CommonMetals
             && this.RareEarthElements >= res.RareEarthElements) {
                return true;
            }

            return false;
        }

        public bool IsStrictlyGreater(Resourses res) {
            if (this.Hydrogen > res.Hydrogen
             && this.CommonMetals > res.CommonMetals
             && this.RareEarthElements > res.RareEarthElements) {
                return true;
            }

            return false;
        }

        public void SetToZero() {
            this.Hydrogen = 0;
            this.CommonMetals = 0;
            this.RareEarthElements = 0;
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
                if (value >= 0) {
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

        public static Resourses Zero => zero;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
