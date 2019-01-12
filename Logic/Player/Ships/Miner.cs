using Logic.Resource;
using System;

namespace Logic.PlayerClasses {
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
            catch (ArgumentException) {
                to.Add(from);
                from.SetToZero();
            }
        }

        public static Resources Price { get => price; }
        public static Resources ExtractsPerTurn { get => extractsPerTurn; }
    }
}
