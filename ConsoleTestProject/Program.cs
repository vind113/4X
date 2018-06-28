using System;
using System.Collections.Generic;
using Logic.GameClasses;
using Logic.Space_Objects;

namespace ConsoleTestProject {
    class Program {
        static void Main(string[] args) {
            //TestGrowthMultipleTimes();
            //TestDate();
            //GenStars();
            //CheckTurns();
            CheckPlanets();
        }

        #region Test growth
        /*private static void TestGrowthMultipleTimes() {
            double sum = 0;
            double testCount = 1;
            for (int i = 0; i < testCount; i++) {
                sum += TestGrowth();
            }
            Console.WriteLine(sum / testCount);
        }

        private static double TestGrowth() {
            int planetNumber = 1000;
            Planet[] planets = new Planet[planetNumber];
            Player player = new Player();

            for (int index = 0; index < planetNumber; index++) {
                planets[index] = Planet.GeneratePlanet("ABC12", PlanetType.Continental);
                planets[index].Colonize();
                //Console.WriteLine(planets[index]);
            }

            double firstPop = 0;
            double lastPop = 0;
            for (int i = 0; i < planets.Length; i++) {
                firstPop += planets[i].Population;
            }
            for (int i = 0; i < 120; i++) {
                foreach (var p in planets) {
                    p.NextTurn(player);
                }
            }
            for (int i = 0; i < planets.Length; i++) {
                lastPop += planets[i].Population;
            }
            Console.WriteLine(firstPop);
            Console.WriteLine(lastPop);
            return lastPop / firstPop;
        }*/
        #endregion

        static void TestDate() {
            for (int i = 0; i < 100; i++) {
                Console.Write(Game.GameDate+"   ");
                Console.WriteLine(Game.GameTurn);
                Game.NextTurn();
            }
            
        }

        static void GenStars() {
            List<Star> stars = new List<Star>();

            for (int i = 0; i < 100_000; i++) {
                stars.Add(Star.GenerateStar($"{i}"));
            }

            foreach (var p in stars) {
                //Console.WriteLine($"{p.Name} {p.Type.Name} {p.Radius}");
                if (p.LumClass == LuminosityClass.A) {
                    Console.WriteLine(p);
                }
            }
        }

        static void CheckTurns() {
            for (int i = 0; i < 100_000; i++) {
                Game.NextTurn();
                if(Game.Player.TotalPopulation != 7_500_000_000) {
                    Console.WriteLine("Incorrect "+ Game.Player.TotalPopulation);
                    break;
                }
            }
        }

        static void CheckPlanets() {
            Planet[] planets = new Planet[1_000_000];
            for (int index = 0; index < planets.Length; index++) {
                planets[index] = Planet.GeneratePlanet("PLA", PlanetTypeValue.Barren);
            }
            int bigCount = 0;
            int midCount = 0;
            int smaCount = 0;

            foreach (var planet in planets) {
                if(planet.Radius >= 11_000) {
                    bigCount++;
                }
                else if(planet.Radius >= 9_000) {
                    midCount++;
                }
                else {
                    smaCount++;
                }
            }
            Console.WriteLine(bigCount);
            Console.WriteLine(midCount);
            Console.WriteLine(smaCount);
            Console.WriteLine();
            Console.WriteLine(bigCount+midCount+smaCount);
        }
    }
}
