using Logic.Resource;
using System;

namespace Logic.PlayerClasses {
    [Serializable]
    public class MinerFleet {
        public static IComparableResources ShipPrice { get; } = new ReadOnlyResources(1E9, 10E9, 100E6);
        public static IComparableResources OneMinerExtractsPerTurn { get; } = new ReadOnlyResources(1E7, 1E8, 1E5);

        public int MinersCount { get; private set; } = 0;

        public MinerFleet() {

        }

        public MinerFleet(int initialMiners) {
            this.MinersCount += initialMiners;
        }

        public void AddMiners(IShipsFactory ships, int quantity) {
            this.MinersCount += ships.GetMiners(quantity);
        }

        public void Mine(IMutableResources from, IMutableResources to) {
            IMutableResources extracted = new Resources(OneMinerExtractsPerTurn);
            extracted.Multiply(this.MinersCount);

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
    }
}
