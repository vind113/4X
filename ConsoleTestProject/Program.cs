using System;

using Logic.GameClasses;

namespace ConsoleTestProject {
    class Program {
        static void Main(string[] args) {
            //TestGrowthMultipleTimes();
            TestDate();

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
    }
}
