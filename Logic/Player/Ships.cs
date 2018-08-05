using Logic.Resourse;
using System;

namespace Logic.PlayerClasses {
    public abstract class Ship {

    }

    public class Colonizer : Ship {
        private readonly static Resourses price = new Resourses(1E9, 1E10, 1E6);
        private const double colonists = 10_000_000;

        private double colonistsOnShip;

        public Colonizer() {
            ColonistsOnShip = colonists;
        }

        public static Resourses Price { get => price; }
        public static double Colonists { get => colonists; }

        public double ColonistsOnShip {
            get => this.colonistsOnShip;
            private set {
                if (value >= 0) {
                    this.colonistsOnShip = value;
                }
            }
        }

        public double GetColonists(double colonists) {
            if (colonists <= this.ColonistsOnShip) {
                this.ColonistsOnShip -= colonists;
                return colonists;
            }

            return 0;
        }

        public static Colonizer TryGetColonizer(Resourses playerResourses) {
            Resourses colonizerCost = Colonizer.Price;

            if (playerResourses == null) {
                throw new ArgumentNullException(nameof(playerResourses));
            }

            if (playerResourses >= colonizerCost) {
                playerResourses.Substract(colonizerCost);
                return new Colonizer();
            }

            return null;
        }
    }

    //tests
    public class Miner : Ship {
        private readonly static Resourses price = new Resourses(1E9, 10E9, 1E8);
        private readonly static Resourses extractsPerTurn = new Resourses(1E7, 1E8, 1E5);

        public static int TryGetMiners(int quantity, Resourses resourses) {
            try {
                Resourses neededResourses = new Resourses(quantity * price.Hydrogen, quantity * price.CommonMetals, quantity * price.RareEarthElements);
                resourses.Substract(neededResourses);
                return quantity;
            }
            catch (ArgumentException) {
                return 0;
            }
        }

        public static void Mine(int quantityOfMiners, Resourses from, Resourses to) {
            Resourses extracted = new Resourses(
                quantityOfMiners * Miner.ExtractsPerTurn.Hydrogen,
                quantityOfMiners * Miner.ExtractsPerTurn.CommonMetals,
                quantityOfMiners * Miner.ExtractsPerTurn.RareEarthElements
            );

            if (from == Resourses.Zero) {
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
        
    }
}
