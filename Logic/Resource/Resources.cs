﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Logic.Resource {
    /// <summary>
    /// Представляет ресурсы
    /// </summary>
    [Serializable]
    public class Resources : INotifyPropertyChanged {
        private double hydrogen;
        private double commonMetals;
        private double rareEarthElements;

        private static readonly Resources zero = new Resources();

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
        /// Инициализирует новый объект <see cref="Resources"/> переданным объектом <see cref="Resources"/>
        /// </summary>
        /// <param name="res">
        /// Объект, с которого создастся новый объект <see cref="Resources"/>
        /// </param>
        public Resources(Resources res)
            :this(res.Hydrogen, res.CommonMetals, res.RareEarthElements) { }

        /// <summary>
        /// Сравнивает два объекта <see cref="Resources"/>
        /// </summary>
        /// <returns>Булевое значение, показывающее, равны ли соответствующие составные объектов <see cref="Resources"/></returns>
        public static bool AreEqual(Resources res1, Resources res2) {
            if (res1.Hydrogen == res2.Hydrogen
             && res1.CommonMetals == res2.CommonMetals
             && res1.RareEarthElements == res2.RareEarthElements) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Сравнивает два объекта <see cref="Resources"/>
        /// </summary>
        /// <returns>Булевое значение, показывающее, отличаются ли соответствующие составные объектов <see cref="Resources"/></returns>
        public static bool AreNotEqual(Resources res1, Resources res2) {
            if (res1.Hydrogen != res2.Hydrogen
             || res1.CommonMetals != res2.CommonMetals
             || res1.RareEarthElements != res2.RareEarthElements) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Прибавляет ресурсы переданного объекта <see cref="Resources"/>
        /// </summary>
        public void Add(Resources parameter) {
            if (!this.CanAdd(parameter)) {
                throw new ArgumentException("Sum of resorses is greater than limit");
            }

            this.Hydrogen          += parameter.Hydrogen;
            this.CommonMetals      += parameter.CommonMetals;
            this.RareEarthElements += parameter.RareEarthElements;
        }

        /// <summary>
        /// Вычитает ресурсы переданного объекта <see cref="Resources"/>
        /// </summary>
        /// <exception cref="ArgumentException"/>
        public void Subtract(Resources parameter) {

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
        public bool CanSubtract(Resources res) {
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
        public bool CanAdd(Resources res) {
            if(double.IsInfinity(this.Hydrogen + res.Hydrogen)
            || double.IsInfinity(this.CommonMetals + res.CommonMetals)
            || double.IsInfinity(this.RareEarthElements + res.RareEarthElements)){
                return false;
            }

            return true;
        }

        public bool IsStrictlyGreater(Resources res) {
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

        public static Resources Zero => zero;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}