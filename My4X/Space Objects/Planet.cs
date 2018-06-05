using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My4X.SpaceObjects {
    enum PlanetType {
        Continental = 100,
        Desert = 30,
        Ocean = 50,
        Paradise = 200,
        Barren = 0,
        GasGiant = 0,
        Ice = 0,
        Tropical = 60
    }

    class Planet {
        double area;        //площадь планеты в км^2
        PlanetType type;    //тип планеты
        double population;  //население в особях
        //коллекция станций на орбите
        //минералы на планете

        public Planet() {

        } 

        public Planet(double area, PlanetType type, double population) {
            this.area = area;
            this.type = type;
            this.population = population;
        }

        public double Area { get => this.area; set => this.area = value; }

        public double Population { get => this.population; set => this.population = value; }

        internal PlanetType Type { get => this.type; set => this.type = value; }

        private static Planet GeneratePlanet() {
            Random random = new Random();
            return new Planet();
        }
    }
}
