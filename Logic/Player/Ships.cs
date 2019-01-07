using Logic.Resource;
using System;

namespace Logic.PlayerClasses {
    public abstract class Ship {

    }

    /// <summary>
    /// Представляет космический корабль-колонизатор
    /// </summary>
    public class Colonizer : Ship {
        private readonly static Resources price = new Resources(1E9, 1E10, 1E6);
        private const double colonists = 10_000_000;

        private double colonistsOnShip;

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
        public double ColonistsOnShip {
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
        public double GetColonists(double colonists) {
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

    public class Miner : Ship {
        private readonly static Resources price = new Resources(1E9, 10E9, 100E6);
        private readonly static Resources extractsPerTurn = new Resources(1E7, 1E8, 1E5);

        public static int GetMiners(int quantity) {
            return quantity;
        }
    
        public static void Mine(int quantityOfMiners, Resources from, Resources to) {
            Resources extracted = new Resources(
                quantityOfMiners * Miner.ExtractsPerTurn.Hydrogen,
                quantityOfMiners * Miner.ExtractsPerTurn.CommonMetals,
                quantityOfMiners * Miner.ExtractsPerTurn.RareEarthElements
            );

            if (Resources.AreEqual(from, Resources.Zero)) {
                return;
            }

            try {
                from.Subtract(extracted);
                to.Add(extracted);
            }
            catch(ArgumentException) {
                to.Add(from);
                from.SetToZero();
            }
        }

        public static Resources Price { get => price; }
        public static Resources ExtractsPerTurn { get => extractsPerTurn; }
    }

    [Serializable]
    public class Ships {
        public Colonizer GetColonizer(Resources from) {
            if (from == null) {
                throw new ArgumentNullException(nameof(from));
            }

            if (from.CanSubtract(Colonizer.Price)) {

                from.Subtract(Colonizer.Price);
                return Colonizer.GetColonizer();
            }

            return null;
        }

        public int GetMiners(Resources from, int quantity) {
            if (from == null) {
                throw new ArgumentNullException(nameof(from));
            }

            Resources neededResources = new Resources(
                quantity * Miner.Price.Hydrogen,
                quantity * Miner.Price.CommonMetals,
                quantity * Miner.Price.RareEarthElements
            );

            if (from.CanSubtract(neededResources)) {
                from.Subtract(neededResources);
                return quantity;
            }
            else {
                return 0;
            }
        }
    }
}
