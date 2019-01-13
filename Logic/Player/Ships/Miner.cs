using Logic.Resource;
using System;

namespace Logic.PlayerClasses {
    public class Miner : Ship {
        private static readonly Resources price = new Resources(1E9, 10E9, 100E6);
        private static readonly Resources extractsPerTurn = new Resources(1E7, 1E8, 1E5);

        public static int GetMiners(int quantity) {
            return quantity;
        }

        public static void Mine(int quantityOfMiners, Resources from, Resources to) {
            Resources extracted = new Resources(ExtractsPerTurn);
            extracted.Multiply(quantityOfMiners);

            if (from.IsEqual(Resources.Zero)) {
                return;
            }

            if (from.CanSubtract(extracted)) {
                from.Subtract(extracted);
                to.Add(extracted);
            }
            else {
                to.Add(from);
                from.SetToZero();
            }
        }

        public static Resources Price { get => price; }
        public static Resources ExtractsPerTurn { get => extractsPerTurn; }
    }
}
