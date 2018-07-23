using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Logic.Resourse;
using Logic.Space_Objects;

namespace Logic.PlayerClasses {
    public class Player {
        private Stockpile stockpile;
        private CitizenHub hub;
        private ObservableCollection<StarSystem> starSystems;

        private int ownedStars = 1;
        private int colonizedPlanets = 1;
        private int ownedPlanets = 8;

        public Player() {
            this.stockpile = new Stockpile();
            this.hub = new CitizenHub();
            this.starSystems = new ObservableCollection<StarSystem>();
        }

        public Player(Stockpile stockpile, CitizenHub hub, IEnumerable<StarSystem> starSystems) {
            this.stockpile = stockpile ?? throw new ArgumentNullException(nameof(stockpile));
            this.hub = hub ?? throw new ArgumentNullException(nameof(hub));
            this.starSystems = new ObservableCollection<StarSystem>(starSystems) ?? throw new ArgumentNullException(nameof(starSystems));
        }

        #region Properties
        public CitizenHub PlayerCitizenHub {
            get => this.hub;
            set => this.hub = value;
        }

        public Resourses OwnedResourses {
            get => this.stockpile.PlayerResourses;
            set => this.stockpile.PlayerResourses = value;
        }

        public double Money {
            get => this.stockpile.Money;
            set => this.stockpile.Money = value;
        }

        public ObservableCollection<StarSystem> StarSystems {
            get => this.starSystems;
            set => this.starSystems = value;
        }

        public double TotalPopulation {
            get {
                double population = 0;
                foreach(var system in StarSystems) {
                    foreach(var planet in system.SystemPlanets) {
                        population += planet.Population;
                    }
                }
                population += this.PlayerCitizenHub.CitizensInHub;
                return population;
            }
        }

        public int ColonizedPlanets {
            get => this.colonizedPlanets;
            set => this.colonizedPlanets = value;
        }

        public int OwnedPlanets {
            get => this.ownedPlanets;
            set => this.ownedPlanets = value;
        }

        public int OwnedStars {
            get => this.ownedStars;
            set => this.ownedStars = value;
        }
        #endregion
    }
}
