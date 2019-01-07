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
        /// Инициализирует новый объект <see cref="Resourses"/> переданным объектом <see cref="Resourses"/>
        /// </summary>
        /// <param name="res">
        /// Объект, с которого создастся новый объект <see cref="Resourses"/>
        /// </param>
        public Resourses(Resourses res)
            :this(res.Hydrogen, res.CommonMetals, res.RareEarthElements) { }

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
            if (!this.CanAdd(parameter)) {
                throw new ArgumentException("Sum of resorses is greater than limit");
            }

            this.Hydrogen          += parameter.Hydrogen;
            this.CommonMetals      += parameter.CommonMetals;
            this.RareEarthElements += parameter.RareEarthElements;
        }

        /// <summary>
        /// Вычитает ресурсы переданного объекта <see cref="Resourses"/>
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public void Substract(Resourses parameter) {

            if (!this.CanSubtract(parameter)) {
                throw new ArgumentException("Argument can't be greater than object");
            }

            this.Hydrogen -= parameter.Hydrogen;
            this.CommonMetals -= parameter.CommonMetals;
            this.RareEarthElements -= parameter.RareEarthElements;
        }

        /// <summary>
        /// Умножает ресурсы в определеное количество раз
        /// </summary>
        /// <param name="multiplier">
        /// Число, указывающее, в сколько раз увеличить ресурсы
        /// </param>
        public void Multiply(double multiplier) {

            this.Hydrogen *= multiplier;
            this.CommonMetals *= multiplier;
            this.RareEarthElements *= multiplier;
            
        }

        /// <summary>
        /// Проверяет, возможно ли вычесть из объекта ресурсов другой объект ресурсов
        /// </summary>
        /// <param name="res">
        /// Ресурс, который вычитается
        /// </param>
        /// <returns>
        /// Логическое значение, показывающее, возможно ли вычесть из объекта ресурсов другой объект ресурсов
        /// </returns>
        public bool CanSubtract(Resourses res) {
            if (this.Hydrogen          >= res.Hydrogen
             && this.CommonMetals      >= res.CommonMetals
             && this.RareEarthElements >= res.RareEarthElements) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Проверяет, возможно ли прибавить один объект ресурсов к второму
        /// </summary>
        /// <param name="res">
        /// Ресурс, который прибавляется
        /// </param>
        /// <returns>
        /// Логическое значение, показывающее, возможно ли прибавить один объект ресурсов к второму
        /// </returns>
        public bool CanAdd(Resourses res) {
            if(double.IsInfinity(this.Hydrogen + res.Hydrogen)
            || double.IsInfinity(this.CommonMetals + res.CommonMetals)
            || double.IsInfinity(this.RareEarthElements + res.RareEarthElements)){
                return false;
            }

            return true;
        }

        public bool IsStrictlyGreater(Resourses res) {
            if (this.Hydrogen          > res.Hydrogen
             && this.CommonMetals      > res.CommonMetals
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

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
