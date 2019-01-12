using Logic.Resource;
using System;

namespace Logic.PlayerClasses {
    [Serializable]
    public class Ships : IShips {
        public Colonizer GetColonizer(IResources from) {
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
