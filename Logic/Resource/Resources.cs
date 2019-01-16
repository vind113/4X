using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Logic.Resource {
    /// <summary>
    /// Представляет ресурсы
    /// </summary>
    [Serializable]
    public class Resources : IMutableObservableResources {
        public static IComparableResources Zero { get; } = new ReadOnlyResources();

        private double hydrogen;
        private double commonMetals;
        private double rareEarthElements;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Инициализирует новый объект <see cref="Resources"/> значениями по умолчанию
        /// </summary>
        public Resources() {
            this.Hydrogen = 0;
            this.CommonMetals = 0;
            this.RareEarthElements = 0;
        }

        /// <summary>
        /// Инициализирует новый объект <see cref="Resources"/> заданными значениями
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
        public Resources(double hydrogen, double commonMetals, double rareEarthElements) {
            this.Hydrogen = hydrogen;
            this.CommonMetals = commonMetals;
            this.RareEarthElements = rareEarthElements;
        }

        /// <summary>
        /// Инициализирует новый объект <see cref="Resources"/> переданным объектом <see cref="IBasicResources"/>
        /// </summary>
        /// <param name="res">
        /// Объект, с которого создастся новый объект <see cref="Resources"/>
        /// </param>
        public Resources(IBasicResources res)
            :this(res.Hydrogen, res.CommonMetals, res.RareEarthElements) {

        }

        /// <summary>
        /// Сравнивает объекты ресурсов
        /// </summary>
        /// <returns>Булевое значение, показывающее, равны ли соответствующие составные объектов ресурсов</returns>
        public bool IsEqual(IBasicResources res) {
            if (this.Hydrogen == res.Hydrogen
             && this.CommonMetals == res.CommonMetals
             && this.RareEarthElements == res.RareEarthElements) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Сравнивает объекты ресурсов
        /// </summary>
        /// <returns>Булевое значение, показывающее, отличаются ли соответствующие составные объектов ресурсов</returns>
        public bool IsNotEqual(IBasicResources res) {
            if (this.Hydrogen != res.Hydrogen
             || this.CommonMetals != res.CommonMetals
             || this.RareEarthElements != res.RareEarthElements) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Прибавляет ресурсы переданного объекта <see cref="IBasicResources"/>
        /// </summary>
        public void Add(IBasicResources res) {
            if (!this.CanAdd(res)) {
                throw new ArgumentException("Sum of resources is greater than limit");
            }

            this.Hydrogen += res.Hydrogen;
            this.CommonMetals += res.CommonMetals;
            this.RareEarthElements += res.RareEarthElements;
        }

        /// <summary>
        /// Вычитает ресурсы переданного объекта <see cref="IBasicResources"/>
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public void Subtract(IBasicResources res) {
            if (!this.CanSubtract(res)) {
                throw new ArgumentException("Argument can't be greater than object");
            }

            this.Hydrogen -= res.Hydrogen;
            this.CommonMetals -= res.CommonMetals;
            this.RareEarthElements -= res.RareEarthElements;
        }

        /// <summary>
        /// Умножает ресурсы в определеное количество раз
        /// </summary>
        /// <param name="multiplier">
        /// Число, указывающее, в сколько раз увеличить ресурсы
        /// </param>
        public void Multiply(double multiplier) {
            if (multiplier < 0) {
                throw new ArgumentException($"{nameof(multiplier)} should be greater than or equal to zero");
            }

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
        public bool CanSubtract(IBasicResources res) {
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
        public bool CanAdd(IBasicResources res) {
            if (double.IsInfinity(this.Hydrogen + res.Hydrogen)
             || double.IsInfinity(this.CommonMetals + res.CommonMetals)
             || double.IsInfinity(this.RareEarthElements + res.RareEarthElements)) {
                return false;
            }

            return true;
        }

        public bool IsStrictlyGreater(IBasicResources res) {
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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
