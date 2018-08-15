using System;

namespace ConsoleTestProject {
    public enum TemperatureClass {
        ///<summary>90K (-183C)</summary>
        Frigid,

        ///<summary>max 170K (-103C)</summary>
        Cold,

        ///<summary>max 250K (-23C)</summary>
        Cool,

        ///<summary>max 330K (56C)</summary>
        Temperate,

        ///<summary>max 500K (226C)</summary>
        Warm,

        ///<summary>max 1000K (726C)</summary>
        Hot
    }

    class Program {
        static void Main(string[] args) {
            Console.WriteLine(TemperatureClass.Cold.ToString());
        }
    }
}
