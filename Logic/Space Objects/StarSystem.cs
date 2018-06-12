﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using Logic.PlayerClasses;

namespace Logic.Space_Objects {
    public class StarSystem {
        string name;
        List<Star> systemStars;
        List<Planet> systemPlanets;

        public StarSystem() {
            this.name = "DefaultSystem";
            this.systemStars = new List<Star>();
            this.systemPlanets = new List<Planet>();
        }

        public StarSystem(string name, List<Star> systemStars, List<Planet> systemPlanets) {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.systemStars = systemStars ?? throw new ArgumentNullException(nameof(systemStars));
            this.systemPlanets = systemPlanets ?? throw new ArgumentNullException(nameof(systemPlanets));
        }

        public string Name { get => this.name; set => this.name = value; }
        public List<Star> SystemStars { get => this.systemStars; }
        public List<Planet> SystemPlanets { get => this.systemPlanets; }
        /*public CompositeCollection CompositeBodies {
            get => new CompositeCollection() { systemStars, systemPlanets };
        }*/

        public static StarSystem GetSolarSystem() {
            List<Planet> planets = new List<Planet>();

            planets.Add(new Planet("Mercury", 2440, "Barren", 0d));
            planets.Add(new Planet("Venus", 6051, "Barren", 0d));
            planets.Add(new Planet("Earth", 6371, "Continental", 7_500_000_000d));
            planets.Add(new Planet("Mars", 3389, "Desert", 0d));

            planets.Add(new Planet("Jupiter", 71_492, "Gas giant", 0d));
            planets.Add(new Planet("Saturn", 60_268, "Gas giant", 0d));
            planets.Add(new Planet("Uranus", 25_559, "Gas giant", 0d));
            planets.Add(new Planet("Neptune", 24_764, "Gas giant", 0d));

            List<Star> stars = new List<Star> { new Star("Sun", 696_392d) };

            StarSystem solarSystem = new StarSystem("Solar System", stars, planets);

            return solarSystem;
        }

        public void NextTurn(Player player) {
            foreach(Planet planet in this.SystemPlanets) {
                planet.NextTurn(player);
            }
            foreach(Star star in this.SystemStars) {
                star.NextTurn();
            }
        }
    }
}
