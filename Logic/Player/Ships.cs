using Logic.Resourse;
using System;

namespace Logic.PlayerClasses {
    public abstract class Ship {

    }

    /// <summary>
    /// Представляет космический корабль-колонизатор
    /// </summary>
    public class Colonizer : Ship {
        private readonly static Resourses price = new Resourses(1E9, 1E10, 1E6);
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
        public static Resourses Price { get => price; }

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
        private readonly static Resourses price = new Resourses(1E9, 10E9, 100E6);
        private readonly static Resourses extractsPerTurn = new Resourses(1E7, 1E8, 1E5);

        public static int GetMiners(int quantity) {
            return quantity;
        }
    
        public static void Mine(int quantityOfMiners, Resourses from, Resourses to) {
            Resourses extracted = new Resourses(
                quantityOfMiners * Miner.ExtractsPerTurn.Hydrogen,
                quantityOfMiners * Miner.ExtractsPerTurn.CommonMetals,
                quantityOfMiners * Miner.ExtractsPerTurn.RareEarthElements
            );

            if (Resourses.AreEqual(from, Resourses.Zero)) {
                return;
            }

            try {
                from.Substract(extracted);
                to.Add(extracted);
            }
            catch(ArgumentException) {
                to.Add(from);
                from.SetToZero();
            }
        }

        public static Resourses Price { get => price; }
        public static Resourses ExtractsPerTurn { get => extractsPerTurn; }
    }

    [Serializable]
    public class Ships {
        public static Colonizer GetColonizerFrom(Resourses resourses) {
            if (resourses.CanSubstract(Colonizer.Price)) {

                resourses.Substract(Colonizer.Price);
                return Colonizer.GetColonizer();
            }

            return null;
        }

        public static int GetMinersFrom(Resourses resourses, int quantity) {

            Resourses neededResourses = new Resourses(
                quantity * Miner.Price.Hydrogen,
                quantity * Miner.Price.CommonMetals,
                quantity * Miner.Price.RareEarthElements
            );

            if (resourses.CanSubstract(neededResourses)) {
                resourses.Substract(neededResourses);
                return quantity;
            }
            else {
                return 0;
            }

        }
    }
}
