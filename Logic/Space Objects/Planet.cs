﻿using System;

using Logic.SupportClasses;
using Logic.PlayerClasses;
using System.Collections.Generic;

namespace Logic.Space_Objects {
    public struct PlanetType {
        private int quality;
        private string name;

        public PlanetType(int quality, string name) {
            this.quality = quality;
            this.name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public int Quality {
            get => this.quality;
            set => this.quality = value;
        }

        public string Name {
            get => this.name;
            set => this.name = value;
        }
    }

    public enum PlanetTypeValue {
        Continental, Barren, Desert, Paradise, Ocean, GasGiant, IceWorld, Tropical, Tundra
    }

    static public class PlanetTypeContainer {
        static Dictionary<PlanetTypeValue, PlanetType> planetTypes = new Dictionary<PlanetTypeValue, PlanetType>();

        static PlanetTypeContainer() {
            planetTypes.Add(PlanetTypeValue.Continental, new PlanetType(100, "Continental"));
            planetTypes.Add(PlanetTypeValue.Barren, new PlanetType(0, "Barren"));
            planetTypes.Add(PlanetTypeValue.Desert, new PlanetType(30, "Desert"));
            planetTypes.Add(PlanetTypeValue.Paradise, new PlanetType(200, "Paradise"));
            planetTypes.Add(PlanetTypeValue.Ocean, new PlanetType(50, "Ocean"));
            planetTypes.Add(PlanetTypeValue.GasGiant, new PlanetType(0, "Gas giant"));
            planetTypes.Add(PlanetTypeValue.IceWorld, new PlanetType(0, "Ice world"));
            planetTypes.Add(PlanetTypeValue.Tropical, new PlanetType(70, "Tropical"));
            planetTypes.Add(PlanetTypeValue.Tundra, new PlanetType(65, "Tundra"));
        }

        public static PlanetType GetPlanetType(PlanetTypeValue key) {
            if (planetTypes.ContainsKey(key)) {
                return planetTypes[key];
            }
            else {
                return planetTypes[PlanetTypeValue.Barren];
            }
        }
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

        public Planet(string name, double radius, PlanetTypeValue type, double population) {
            this.name = name;
            this.radius = radius;
            this.type = PlanetTypeContainer.GetPlanetType(type);

            this.area = Math.Floor(HelperMathFunctions.SphereArea(this.radius));

            this.maximumPopulation = (double)this.type.Quality * this.area;

            this.buildingSites = (int)(this.MaximumPopulation / 100_000_000d);
            this.availableSites = this.buildingSites;
           
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

        #region Planet Generation
        public static Planet GeneratePlanet(string name, PlanetTypeValue planetType) {
            string planetName = name;
            double population = 0;
            double radius = 0;

            if(planetType == PlanetTypeValue.GasGiant) {
                radius = GasGiantRadiusGeneration();
            }
            else {
                radius = RockyPlanetRadiusGeneration();
            }

            return new Planet(planetName, radius, planetType, population);
        }

        private static double RockyPlanetRadiusGeneration() {
            double radius;

            if (HelperRandomFunctions.PercentProbableBool(3)) {
                radius = (double)HelperRandomFunctions.GetRandomInt(11_000, 13_000);
            }
            else if (HelperRandomFunctions.PercentProbableBool(10)) {
                radius = (double)HelperRandomFunctions.GetRandomInt(9_000, 11_000);
            }
            else {
                radius = (double)HelperRandomFunctions.GetRandomInt(3_000, 9_000);
            }

            return radius;
        }

        private static double GasGiantRadiusGeneration() {
            double radius;
            if (HelperRandomFunctions.PercentProbableBool(15)) {
                radius = (double)HelperRandomFunctions.GetRandomInt(100_000, 150_000);
            }
            else {
                radius = (double)HelperRandomFunctions.GetRandomInt(20_000, 100_000);
            }
            return radius;
        }
        #endregion

        public override string ToString() {
            return $"{this.Name} is a {this.Type.Name} world with radius of {this.radius} km " +
                $"and area of {this.Area:E4} km^2. " +
                $"Here lives {this.Population:E4} intelligent creatures. " +
                $"On this planet can live {this.maximumPopulation:E4} people. " +
                $"We can build {this.buildingSites} buildings here. ";
        }

        public void NextTurn(Player player) {
            //AddPopulation();

            //тестируй
            CitizensToTheHub(player);
            CitizensFromTheHub(player);
        }

        #region Next turn functions 
        private void AddPopulation() {

            double partOfGrowth = 5_000d;

            if (this.Population > 0) {
                double growthCoef = (this.MaximumPopulation / this.Population) / (partOfGrowth);

                double newPopulation =
                    HelperRandomFunctions.GetRandomDouble() * growthCoef * ((double)this.Type.Quality/100d) * this.population;

                newPopulation = Math.Ceiling(newPopulation);
                this.Population += newPopulation;
            }
        }

        private void CitizensToTheHub(Player player) {
            if (this.Population > 0) {
                double travellersExpected = Math.Floor(this.Population * 0.002);

                double travellersReal =
                    Math.Ceiling(travellersExpected * HelperRandomFunctions.GetRandomDouble());

                bool canTravelFromPlanet = this.Population > travellersReal;
                bool canTravelToHub =
                    (player.PlayerCitizenHub.CitizensInHub + travellersReal) < player.PlayerCitizenHub.MaximumCount;

                if (canTravelFromPlanet && canTravelToHub) {
                    this.Population -= travellersReal;
                    player.PlayerCitizenHub.CitizensInHub += travellersReal;
                }
            }
        }

        private void CitizensFromTheHub(Player player) {
            if (this.Population > 0) {
                double addedPopulation =
                    Math.Ceiling(HelperRandomFunctions.GetRandomDouble() * player.PlayerCitizenHub.CitizensInHub);

                bool canTravelToPlanet = (this.Population + addedPopulation) < this.MaximumPopulation;
                bool canTravelFromHub = (player.PlayerCitizenHub.CitizensInHub > addedPopulation);

                if (canTravelToPlanet && canTravelFromHub) {

                    player.PlayerCitizenHub.CitizensInHub -= addedPopulation;
                    this.Population += addedPopulation;
                }
            }
        }
        #endregion

        public void Colonize() {
            if (this.Population == 0) {
                this.Population += 10_000_000;
            }
        }
    }
}
