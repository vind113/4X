using System;

using Logic.SupportClasses;

namespace Logic.Space_Objects {

    //описывает тип планеты
    public enum PlanetType {
        Continental = 100,
        Desert = 30,
        Ocean = 50,
        Paradise = 200,
        Barren = 0,
        GasGiant = 0,
        Ice = 0,
        Tropical = 60
    }

    [Serializable]
    public class Planet : CelestialBody {
        PlanetType type;            //тип планеты
        double population;          //население в особях
        int buildingSites;          //количество мест для строительствы
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
            private set {
                if (this.maximumPopulation > value) {
                    this.population = value;
                };
            }
        }
        public int BuildingSites { get => this.buildingSites; }
        public int AvailableSites { get => this.availableSites; }
        public double MaximumPopulation { get => this.maximumPopulation; }
        public PlanetType Type { get => this.type; }


        public static Planet GeneratePlanet() {
            Random random = new Random();
            string planetName = "ABC87";
            double radius = (double)random.Next(3000, 10000);
            double population = random.NextDouble() * 10_000_000_000;
            return new Planet(planetName, radius, PlanetType.Continental, population);
        }

        public override string ToString() {
            return $"{this.Name} is a {this.Type} world with radius of {this.radius} km " +
                $"and area of {this.Area:E4} km^2. " +
                $"Here lives {this.Population:E4} intelligent creatures. " +
                $"On this planet can live {this.maximumPopulation} people. " +
                $"We can build {this.buildingSites} buildings here. ";
        }

        public void NextTurn() {
            AddPopulation();
            //CitizensToTheHub();
            //CitizensFromTheHub();
        }

        #region Next turn functions 
        private void AddPopulation() {
            double growthCoef = (this.MaximumPopulation / this.Population) / 100_000;
            double newPopulation = HelperRandomFunctions.GetRandomDouble() * growthCoef * (double)this.Type * this.population;
            newPopulation = Math.Ceiling(newPopulation);
            this.Population += newPopulation;
        }

        private void CitizensToTheHub() {
            double travellersExpected = Math.Ceiling(this.Population / 5000);
            double travellersReal = Math.Ceiling(travellersExpected * HelperRandomFunctions.GetRandomDouble());
            double newPopulation = this.Population - travellersReal;
            this.Population = newPopulation;
        }

        private void CitizensFromTheHub() {

        }


        #endregion

        public void Colonize() {

        }
    }
}
