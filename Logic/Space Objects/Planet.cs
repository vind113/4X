using System;
using System.ComponentModel;
using Logic.SupportClasses;
using Logic.PlayerClasses;
using System.Collections.Generic;
using Logic.Resourse;

namespace Logic.Space_Objects {
    public struct PlanetType {
        private const int goodWorldQuality = 100;

        //TODO: СДЕЛАЙ БАЙТОМ
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

        public static int GoodWorldQuality { get => goodWorldQuality; }
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
    public class Planet : CelestialBody {
        private PlanetType type;            //тип планеты
        private double population;          //население в особях
        private int buildingSites;          //количество мест для строительства
        private int availableSites;         //количество доступных мест для строительства
        private double maximumPopulation;   //максимальное население планеты

        private const double citizensPerSector = 100_000_000d;

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

            this.buildingSites = (int)(this.MaximumPopulation / citizensPerSector);
            this.availableSites = this.buildingSites;
           
            this.population = Math.Floor(population);

            this.BodyResourse = GetPlanetResourses(planetType, planetArea);
        }

        private Resourses GetPlanetResourses(PlanetType planetType, double planetArea) {
            double commonMetals = 0;
            double rareEarthElements = 0;
            double hydrogen = 0;

            if(planetType.Name != "Gas giant") {
                const double massOfTenKmCrust = (10d * ((3d) * 10E9));
                commonMetals = planetArea * (massOfTenKmCrust / 20);
                rareEarthElements = planetArea * (massOfTenKmCrust / 1E5);
                hydrogen = planetArea / 10;
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
                    OnPropertyChanged();
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

            double partOfGrowth = 0.0015d;

            double growthCoef = partOfGrowth * HelperRandomFunctions.GetRandomDouble();

            double addedPart =
                growthCoef * ((double)this.Type.Quality / (double)PlanetType.GoodWorldQuality);

            double addedPopulation = this.Population * addedPart;
            this.Population += addedPopulation;
        }

        private void CitizensToTheHub(Player player) {
            if (this.Population == 0) {
                return;
            }

            double partOfTravellers = 1_000d;

            double citizensToHubExpected = Math.Floor(this.Population / partOfTravellers);

            double citizensToHub =
                Math.Floor(citizensToHubExpected * HelperRandomFunctions.GetRandomDouble());

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
                Math.Floor(HelperRandomFunctions.GetRandomDouble() * player.PlayerCitizenHub.CitizensInHub);

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

            double miningDifficulty = ((double)this.Type.Quality / (double)PlanetType.GoodWorldQuality) / 20;

            double miningCoef = miningDifficulty * this.Population;
            ExtractAllRseourses(player, miningCoef);
        }

        private void ExtractAllRseourses(Player player, double miningCoef) {
            var resultTuple =
                this.ExtractOneResourse(miningCoef, this.BodyResourse.Hydrogen, player.OwnedResourses.Hydrogen);
            this.BodyResourse.Hydrogen = resultTuple.Item1;
            player.OwnedResourses.Hydrogen = resultTuple.Item2;

            resultTuple =
                this.ExtractOneResourse(miningCoef, this.BodyResourse.CommonMetals, player.OwnedResourses.CommonMetals);
            this.BodyResourse.CommonMetals = resultTuple.Item1;
            player.OwnedResourses.CommonMetals = resultTuple.Item2;

            resultTuple =
                this.ExtractOneResourse(miningCoef, this.BodyResourse.RareEarthElements, player.OwnedResourses.RareEarthElements);
            this.BodyResourse.RareEarthElements = resultTuple.Item1;
            player.OwnedResourses.RareEarthElements = resultTuple.Item2;
        }

        private Tuple<double, double> ExtractOneResourse(double miningCoef, double resourseOnPlanet, double resourseInPosession) {
            if (resourseOnPlanet <= 0) {
                return new Tuple<double, double>(0, resourseInPosession);
            }

            double resourseExtracted = 0;
            resourseExtracted = miningCoef;

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
        /// <returns>
        /// Булевое значение, которое показывает успешность колонизации
        /// </returns>
        public bool Colonize(Player player) {
            double colonizers = 10_000_000;

            if (this.Population == 0 && this.MaximumPopulation > colonizers) {

                this.Population += colonizers;
                player.ColonizedPlanets++;

                return true;
            }

            return false;
        }
    }
}
