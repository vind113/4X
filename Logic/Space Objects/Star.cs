using Logic.SupportClasses;
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
        /// Инициализирует экземпляр класса планеты с значениями по умолчанию
        /// </summary>
        public Star() {

        }

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
        public Star(string name, double radius, LuminosityClass luminosityClass) {
            this.name = name;
            this.radius = radius;

            this.lumClass = luminosityClass;

            this.area = HelperMathFunctions.SphereArea(this.Radius);
        }

        /// <summary>
        /// Сгенерировать звезду с заданым именем
        /// </summary>
        /// <param name="name">
        ///     Имя звезды
        /// </param>
        /// <returns>
        ///     Возвращает экземпляр класса <see cref="Star"/>
        /// </returns>
        public static Star GenerateStar(string name) {
            int radius = 0;
            LuminosityClass luminosityClass;

            double starFraction = HelperRandomFunctions.GetRandomDouble();

            if (starFraction < 0.0003) {
                radius = HelperRandomFunctions.GetRandomInt(4_620_000, 10_000_000);
                luminosityClass = LuminosityClass.O;

            }
            else if (starFraction < 0.0013) {
                radius = HelperRandomFunctions.GetRandomInt(1_260_000, 4_620_000);
                luminosityClass = LuminosityClass.B;

            }
            else if (starFraction < 0.006) {
                radius = HelperRandomFunctions.GetRandomInt(805_000, 1_260_000);
                luminosityClass = LuminosityClass.A;

            }
            else if (starFraction < 0.03) {
                radius = HelperRandomFunctions.GetRandomInt(728_000, 805_000);
                luminosityClass = LuminosityClass.F;

            }
            //реальное соотношение 0.076
            else if (starFraction < 0.2) {
                radius = HelperRandomFunctions.GetRandomInt(672_000, 728_000);
                luminosityClass = LuminosityClass.G;

            }
            //реальное соотношение 0.12
            else if (starFraction < 0.3) {
                radius = HelperRandomFunctions.GetRandomInt(490_000, 672_000);
                luminosityClass = LuminosityClass.K;

            }
            else {
                radius = HelperRandomFunctions.GetRandomInt(360_000, 490_000);
                luminosityClass = LuminosityClass.M;
            }

            return new Star(name, radius, luminosityClass);
        }

        /// <summary>
        ///     Преобразует данные экземпляра в строковое представление
        /// </summary>
        /// <returns>Строку, представляющую объект</returns>
        public override string ToString() {
            return $"It is a star called {this.name}" +
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
