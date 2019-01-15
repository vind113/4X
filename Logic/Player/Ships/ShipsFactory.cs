using Logic.Resource;
using System;

namespace Logic.PlayerClasses {
    [Serializable]
    public class ShipsFactory : IShipsFactory {
        private IMutableResources storage;

        public ShipsFactory(IMutableResources resources) {
            storage = resources;
        }

        public Colonizer GetColonizer() {
            if (storage.CanSubtract(Colonizer.Price)) {

                storage.Subtract(Colonizer.Price);
                return Colonizer.GetColonizer();
            }

            return null;
        }

        public int GetMiners(int quantity) {
            Resources neededResources = new Resources(MinerFleet.ShipPrice);
            neededResources.Multiply(quantity);

            if (storage.CanSubtract(neededResources)) {
                storage.Subtract(neededResources);
                return quantity;
            }
            else {
                return 0;
            }
        }
    }
}
