using Logic.Resource;
using System;

namespace Logic.PlayerClasses {
    [Serializable]
    public class Ships : IShips {
        private IResources storage;

        public Ships(IResources resources) {
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
            Resources neededResources = new Resources(Miner.Price);
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
