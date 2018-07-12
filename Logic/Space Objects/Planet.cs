using System;

using Logic.SupportClasses;
using Logic.PlayerClasses;
using System.Collections.Generic;
using Logic.Resourse;

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
            planetTypes.Add(PlanetTypeValue.Continental, new PlanetType(90, "Continental"));
            planetTypes.Add(PlanetTypeValue.Barren, new PlanetType(0, "Barren"));
            planetTypes.Add(PlanetTypeValue.Desert, new PlanetType(30, "Desert"));
            planetTypes.Add(PlanetTypeValue.Paradise, new PlanetType(150, "Paradise"));
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

    /// <summary>
    /// Представляет планету
    /// </summary>
    [Serializable]
    public class Planet : CelestialBody {
        PlanetType type;            //тип планеты
        double population;          //население в особях
        int buildingSites;          //количество мест для строительства
        int availableSites;         //количество доступных мест для строительства
        double maximumPopulation;   //максимальное население планеты
        //коллекция станций на орбите

        /// <summary>
        /// Инициализирует экземпляр класса планеты с значениями по умолчанию
        /// </summary>
        public Planet() {
            
        }

        /// <summary>
        /// Инициализирует экземпляр класса планеты
        /// </summary>
        /// <param name="name">
        ///     Имя планеты
        /// </param>
        /// <param name="radius">
        ///     Радуис планеты
        /// </param>
        /// <param name="type">
        ///     Тип планеты(пустынная, океаническая, газовый гигант и т.д)
        /// </param>
        /// <param name="population">
        ///     Изначальное население планеты
        /// </param>
        public Planet(string name, double radius, PlanetTypeValue type, double population) {
            this.name = name;
            this.radius = radius;

            PlanetType planetType = PlanetTypeContainer.GetPlanetType(type);
            this.type = planetType;

            double planetArea = Math.Floor(HelperMathFunctions.SphereArea(this.radius));
            this.area = planetArea;

            this.maximumPopulation = (double)this.type.Quality * this.area;

            this.buildingSites = (int)(this.MaximumPopulation / 100_000_000d);
            this.availableSites = this.buildingSites;
           
            this.population = Math.Floor(population);

            this.BodyResourse = GetPlanetResourses(planetType, planetArea);
        }

        private Resourses GetPlanetResourses(PlanetType planetType, double planetArea) {
            double commonMetals = 0;
            double rareEarthElements = 0;
            double hydrogen = 0;

            if(planetType.Name != "Gas giant") {
                commonMetals = HelperRandomFunctions.GetRandomInt(70, 130) * planetArea * 10;
                rareEarthElements = (HelperRandomFunctions.GetRandomInt(90, 110) * planetArea) / 2;
                hydrogen = (HelperRandomFunctions.GetRandomInt(70, 100) * planetArea) / 10;
            }

            Resourses planetResourses = new Resourses(hydrogen, commonMetals, rareEarthElements);

            return planetResourses;
        }

        /// <summary>
        /// Население планеты
        /// </summary>
        public double Population {
            get => this.population;
            //private set нужен для централизованой проверки на допустимость значения внутри класса
            private set {
                if (this.maximumPopulation > value && value > 0) {
                    this.population = value;
                };
            }
        }

        /// <summary>
        /// Общее количество строительных площадок
        /// </summary>
        public int BuildingSites { get => this.buildingSites; }

        /// <summary>
        /// Количество доступных для строительства площадок
        /// </summary>
        public int AvailableSites { get => this.availableSites; }

        /// <summary>
        /// Максимально возможное количество жителей на планете
        /// </summary>
        public double MaximumPopulation { get => this.maximumPopulation; }

        /// <summary>
        /// Тип планеты
        /// </summary>
        public PlanetType Type { get => this.type; }

        #region Planet Generation
        /// <summary>
        ///     Генерирует планету с заданым именем и типом
        /// </summary>
        /// <param name="name">
        ///     Имя планеты
        /// </param>
        /// <param name="planetType">
        ///     Тип планеты(пустынная, океаническая, газовый гигант и т.д)
        /// </param>
        /// <returns></returns>
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

            if (HelperRandomFunctions.PercentProbableBool(8)) {
                radius = (double)HelperRandomFunctions.GetRandomInt(11_000, 13_000);
            }
            else if (HelperRandomFunctions.PercentProbableBool(15)) {
                radius = (double)HelperRandomFunctions.GetRandomInt(9_000, 11_000);
            }
            else {
                radius = (double)HelperRandomFunctions.GetRandomInt(3_000, 9_000);
            }

            return radius;
        }

        private static double GasGiantRadiusGeneration() {
            double radius;
            if (HelperRandomFunctions.PercentProbableBool(10)) {
                radius = (double)HelperRandomFunctions.GetRandomInt(110_000, 150_000);
            }
            else if (HelperRandomFunctions.PercentProbableBool(20)) {
                radius = (double)HelperRandomFunctions.GetRandomInt(100_000, 110_000);
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

        #region Next turn functions
        /// <summary>
        ///     Выполняет все операции для перехода на следующий ход
        /// </summary>
        /// <param name="player">
        ///     Игрок, которому принадлежит планета
        /// </param>
        public void NextTurn(Player player) {
            AddPopulation();
        
            CitizensToTheHub(player);
            CitizensFromTheHub(player);

            ExtractResourses(player);
        }

        private void AddPopulation() {
            if (this.Population == 0) {
                return;
            }

            double partOfGrowth = 7_000d;

            double growthCoef = (this.MaximumPopulation / this.Population) / (partOfGrowth);

            double newPopulation =
                HelperRandomFunctions.GetRandomDouble() * growthCoef * ((double)this.Type.Quality / 100d) * this.population;

            newPopulation = Math.Ceiling(newPopulation);
            this.Population += newPopulation;
        }

        private void CitizensToTheHub(Player player) {
            if (this.Population == 0) {
                return;
            }

            double partOfTravellers = 1_000;

            double citizensToHubExpected = Math.Floor(this.Population / partOfTravellers);

            double citizensToHub =
                Math.Ceiling(citizensToHubExpected * HelperRandomFunctions.GetRandomDouble());

            bool canTravelFromPlanet = citizensToHub < this.Population;
            bool canTravelToHub =
                (player.PlayerCitizenHub.CitizensInHub + citizensToHub) < player.PlayerCitizenHub.MaximumCount;

            if (canTravelFromPlanet && canTravelToHub) {
                this.Population -= citizensToHub;
                player.PlayerCitizenHub.CitizensInHub += citizensToHub;
            }      
        }

        private void CitizensFromTheHub(Player player) {
            if (this.Population == 0) {
                return;
            }

            double citizensFromHub =
                Math.Ceiling(HelperRandomFunctions.GetRandomDouble() * player.PlayerCitizenHub.CitizensInHub);

            bool canTravelToPlanet = (this.Population + citizensFromHub) < this.MaximumPopulation;
            bool canTravelFromHub = citizensFromHub < player.PlayerCitizenHub.CitizensInHub;

            if (canTravelToPlanet && canTravelFromHub) {
                player.PlayerCitizenHub.CitizensInHub -= citizensFromHub;
                this.Population += citizensFromHub;
            }
        }

        private void ExtractResourses(Player player) {
            if (this.Population == 0) {
                return;
            }

            //случайный модификатор добычи
            double partOfExtraction = (double)HelperRandomFunctions.GetRandomInt(1, 5) / 3;

            double gameTurnsToDepletion = 3000;
            double miningDifficulty = (double)this.Type.Quality / 100;

            double miningCoef = miningDifficulty * partOfExtraction / gameTurnsToDepletion;
            ExtractAllRseourses(player, miningCoef);
        }

        private void ExtractAllRseourses(Player player, double miningCoef) {
            var resultTuple =
                this.ExtractOneResourse(miningCoef, this.BodyResourse.Hydrogen, player.PlayerResourses.Hydrogen);
            this.BodyResourse.Hydrogen = resultTuple.Item1;
            player.PlayerResourses.Hydrogen = resultTuple.Item2;

            resultTuple =
                this.ExtractOneResourse(miningCoef, this.BodyResourse.CommonMetals, player.PlayerResourses.CommonMetals);
            this.BodyResourse.CommonMetals = resultTuple.Item1;
            player.PlayerResourses.CommonMetals = resultTuple.Item2;

            resultTuple =
                this.ExtractOneResourse(miningCoef, this.BodyResourse.RareEarthElements, player.PlayerResourses.RareEarthElements);
            this.BodyResourse.RareEarthElements = resultTuple.Item1;
            player.PlayerResourses.RareEarthElements = resultTuple.Item2;
        }

        private Tuple<double, double> ExtractOneResourse(double miningCoef, double resourseOnPlanet, double resourseInPosession) {
            if (resourseOnPlanet <= 0) {
                return new Tuple<double, double>(0, resourseInPosession);
            }

            double resourseExtracted = 0;
            resourseExtracted = resourseOnPlanet * miningCoef;

            if (resourseExtracted <= resourseOnPlanet && resourseExtracted > 1E5) {
                resourseInPosession += resourseExtracted;
                resourseOnPlanet -= resourseExtracted;
            }
            else {
                resourseInPosession += resourseOnPlanet;
                resourseOnPlanet = 0;
            }

            return new Tuple<double, double>(resourseOnPlanet, resourseInPosession);
        }
        #endregion

        /// <summary>
        ///     Выполняет все операции для колонизации планеты(при соблюдении необходимых условий)
        /// </summary>
        /// <param name="player">
        ///     Игрок, который колонизирует планету
        /// </param>
        public void Colonize(Player player) {
            double colonizers = 10_000_000;

            if (this.Population == 0 && this.MaximumPopulation > colonizers) {

                this.Population += colonizers;
                player.ColonizedPlanets++;
            }
        }
    }
}
