﻿using System;

using Logic.SupportClasses;
using Logic.PlayerClasses;
using System.Collections.Generic;

namespace Logic.Space_Objects {
    //описывает тип планеты
    //Dictionary or hash table
    public enum PlanetType {
        Continental = 100,
        Desert = 30,
        Ocean = 50,
        Paradise = 200,
        Barren = 0,
        GasGiant = 0,
        Ice = 0,
        Tropical = 70,
        Tundra = 65
    }

    [Serializable]
    public class Planet : CelestialBody {
        PlanetType type;            //тип планеты
        double population;          //население в особях
        int buildingSites;          //количество мест для строительства
        int availableSites;         //количество доступных мест для строительства
        double maximumPopulation;   //максимальное население планеты
        //коллекция станций на орбите

        public Planet() {

        }

        public Planet(string name, double radius, PlanetType type, double population) {
            this.name = name;
            this.radius = radius;
            this.type = type;

            this.area = Math.Floor(HelperMathFunctions.SphereArea(this.radius));
            this.buildingSites = (int)(this.area / 1_000_000d);
            this.availableSites = this.buildingSites;
            this.maximumPopulation = (double)this.type * this.area;

            this.population = Math.Floor(population);
        }

        public double Population {
            get => this.population;
            //private set нужен для централизованой проверки на допустимость значения внутри класса
            private set {
                if (this.maximumPopulation > value && value > 0) {
                    this.population = value;
                };
            }
        }
        public int BuildingSites { get => this.buildingSites; }
        public int AvailableSites { get => this.availableSites; }
        public double MaximumPopulation { get => this.maximumPopulation; }
        public PlanetType Type { get => this.type; }


        public static Planet GeneratePlanet(string name, PlanetType planetType) {
            string planetName = name;
            double population = 0;
            double radius = 0;
            radius = (double)HelperRandomFunctions.GetRandomInt(4000, 10000);
            return new Planet(planetName, radius, planetType, population);
        }

        public override string ToString() {
            return $"{this.Name} is a {this.Type} world with radius of {this.radius} km " +
                $"and area of {this.Area:E4} km^2. " +
                $"Here lives {this.Population:E4} intelligent creatures. " +
                $"On this planet can live {this.maximumPopulation} people. " +
                $"We can build {this.buildingSites} buildings here. ";
        }

        public void NextTurn(Player player) {
            AddPopulation();
            CitizensToTheHub(player);
            CitizensFromTheHub(player);
        }

        #region Next turn functions 
        private void AddPopulation() {
            double growthCoef = (this.MaximumPopulation / this.Population) / 500_000;
            double newPopulation = HelperRandomFunctions.GetRandomDouble() * growthCoef * (double)this.Type * this.population;
            newPopulation = Math.Floor(newPopulation);
            this.Population += newPopulation;
        }

        private void CitizensToTheHub(Player player) {
            /*double travellersExpected = Math.Floor(this.Population * 0.002);

            double travellersReal =
                Math.Ceiling(travellersExpected * HelperRandomFunctions.GetRandomDouble());
            double newPopulation = this.Population - travellersReal;

            player.PlayerCitizenHub.CitizensInHub += travellersReal;

            this.Population = newPopulation; */
        }

        private void CitizensFromTheHub(Player player) {
            /*double addedPopulation =
                Math.Ceiling(HelperRandomFunctions.GetRandomDouble() * player.PlayerCitizenHub.CitizensInHub);

            player.PlayerCitizenHub.CitizensInHub -= addedPopulation;
            this.Population += addedPopulation;*/
        }
        #endregion

        public void Colonize() {
            this.Population += 10_000_000;
        }
    }
}
