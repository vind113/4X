using Logic.Resourse;

namespace Logic.PlayerClasses {
    public class Player {
        private Stockpile stockpile;
        private CitizenHub hub;

        public Player() {
            this.stockpile = new Stockpile();
            this.hub = new CitizenHub();
        }

        #region Properties
        public CitizenHub PlayerCitizenHub {
            get => this.hub;
            set => this.hub = value;
        }

        public Resourses PlayerResourses {
            get => this.stockpile.PlayerResourses;
            set => this.stockpile.PlayerResourses = value;
        }

        public double PlayerMoney {
            get => this.stockpile.Money;
            set => this.stockpile.Money = value;
        }
        #endregion
    }
}
