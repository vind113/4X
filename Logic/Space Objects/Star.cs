using System;

namespace Logic.SpaceObjects {
    /// <summary>
    /// Представляет классы светимости звезды
    /// </summary>
    public enum LuminosityClass {
        O, B, A, F, G, K, M
    }

    /// <summary>
    /// Представляет звезду
    /// </summary>
    [Serializable]
    public class Star : CelestialBody {
        private LuminosityClass lumClass;

        /// <summary>
        /// Класс светимости этой звезды
        /// </summary>
        public LuminosityClass LumClass { get => this.lumClass; }

        /// <summary>
        /// Инициализирует экземпляр класса звезды
        /// </summary>
        /// <param name="name">
        ///     Имя звезды
        /// </param>
        /// <param name="radius">
        ///     Радиус звезды
        /// </param>
        /// <param name="luminosityClass">
        ///     Класс светимости звезды
        /// </param>
        public Star(string name, double radius, LuminosityClass luminosityClass):base(name, radius) {
            this.lumClass = luminosityClass;
        }

        /// <summary>
        ///     Преобразует данные экземпляра в строковое представление
        /// </summary>
        /// <returns>Строку, представляющую объект</returns>
        public override string ToString() {
            return $"It is a star called {this.Name}" +
                $" with radius of {this.Radius:E4} km and area of {this.Area:E4} km^2." +
                $"Luminosity class is {this.LumClass}";
        }

        /// <summary>
        ///     Выполняет все операции для перехода на следующий ход
        /// </summary>
        public void NextTurn() {

        }
    }
}
