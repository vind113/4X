using System;
using System.Collections.Generic;

using Logic.Resourse;
using Logic.Space_Objects;

namespace Logic.PlayerClasses {
    public class Player {
        private Stockpile stockpile;
        private CitizenHub hub;
        private List<StarSystem> starSystems;

        public Player() {
            this.stockpile = new Stockpile();
            this.hub = new CitizenHub();
            this.starSystems = new List<StarSystem>();
        }

        public Player(Stockpile stockpile, CitizenHub hub, List<StarSystem> starSystems) {
            this.stockpile = stockpile ?? throw new ArgumentNullException(nameof(stockpile));
            this.hub = hub ?? throw new ArgumentNullException(nameof(hub));
            this.starSystems = starSystems ?? throw new ArgumentNullException(nameof(starSystems));
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

        public List<StarSystem> StarSystems {
            get => this.starSystems;
            set => this.starSystems = value;
        }
        #endregion
    }
}
