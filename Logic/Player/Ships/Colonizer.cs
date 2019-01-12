using Logic.Resource;

namespace Logic.PlayerClasses {
    /// <summary>
    /// Представляет космический корабль-колонизатор
    /// </summary>
    public class Colonizer : Ship {
        private readonly static Resources price = new Resources(1E9, 1E10, 1E6);
        private const long colonists = 10_000_000;

        private long colonistsOnShip;

        /// <summary>
        /// Инициализирует новый объект <see cref="Colonizer" значениями по умолчанию/>
        /// </summary>
        public Colonizer() {
            ColonistsOnShip = colonists;
        }

        /// <summary>
        /// Возвращает цену колонизатора
        /// </summary>
        public static Resources Price { get => price; }

        /// <summary>
        /// Возвращает количество колонистов на корабле по умолчанию
        /// </summary>
        public static double Colonists { get => colonists; }

        /// <summary>
        /// Возвращает текущее количество колонистов на корабле
        /// </summary>
        public long ColonistsOnShip {
            get => this.colonistsOnShip;
            private set {
                if (value >= 0) {
                    this.colonistsOnShip = value;
                }
            }
        }

        /// <summary>
        /// Вычитает указаное количество колонистов
        /// </summary>
        /// <param name="colonists">Число колонистов, которое необходимо вычесть</param>
        /// <returns>Количество вычтеных колонистов</returns>
        public double GetColonists(long colonists) {
            if (colonists <= this.ColonistsOnShip) {
                this.ColonistsOnShip -= colonists;
                return colonists;
            }

            return 0;
        }

        /// <summary>
        /// Cтроит новый колонизатор
        /// </summary>
        /// <returns>Новый экземпляр <see cref="Colonizer"/></returns>
        public static Colonizer GetColonizer() {
            return new Colonizer();
        }
    }
}
