using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameLogic.Resourse;
using GameLogic.SupportClasses;

namespace GameLogic.Space_Objects {
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

    public class Planet : CelestialBody {
        string name;
        PlanetType type;            //тип планеты
        double population;          //население в особях
        //коллекция станций на орбите
        Resourses planetResourse;   //ресурсы на планете

        public Planet() {

        }

        public Planet(string name, double radius, PlanetType type, double population) {
            this.name = name;
            this.radius = radius;
            this.area = HelperMathFunctions.SphereArea(this.radius);
            this.type = type;
            this.population = Math.Floor(population);
        }


        public double Area { get => this.area; }

        public double Population { get => this.population; }

        public string Name { get => this.name; set => this.name = value; }

        internal PlanetType Type { get => this.type; }

        internal Resourses PlanetResourse { get => this.planetResourse; }


        public static Planet GeneratePlanet() {
            Random random = new Random();
            string planetName = "Name of Planet";
            double radius = random.NextDouble() * 10000;
            double population = random.NextDouble() * 10_000_000_000;
            return new Planet(planetName, radius, PlanetType.Continental, population);
        }

        public override string ToString() {
            return $"{this.Name} is a {this.Type} world with radius of {this.radius} km" +
                $" and area of {this.Area} km^2. Here lives {this.Population} intelligent creatures";
        }
    }
}
