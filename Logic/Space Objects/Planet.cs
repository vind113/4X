using System;
using System.ComponentModel;
using Logic.SupportClasses;
using Logic.PlayerClasses;
using System.Collections.Generic;
using Logic.Resourse;

namespace Logic.SpaceObjects {
    public struct PlanetType {
        private const int goodWorldQuality = 100;

        private byte quality;
        private string name;
        private double miningDifficulty;

        public PlanetType(byte quality, string name) {
            this.quality = quality;
            this.name = name ?? throw new ArgumentNullException(nameof(name));

            double months = 12;
            this.miningDifficulty = ((double)quality / (double)PlanetType.GoodWorldQuality) / months;
        }

        public byte Quality {
            get => this.quality;
            set => this.quality = value;
        }

        public string Name {
            get => this.name;
            set => this.name = value;
        }

        public static int GoodWorldQuality { get => goodWorldQuality; }
        public double MiningDifficulty { get => this.miningDifficulty; }
    }

    public enum PlanetTypeValue {
        Continental, Barren, Desert, Paradise, Ocean, GasGiant, IceWorld, Tropical, Tundra
    }

    static public class PlanetTypeContainer {
        static Dictionary<PlanetTypeValue, PlanetType> planetTypes = new Dictionary<PlanetTypeValue, PlanetType>();
        static PlanetType[] planetTypesArray = new PlanetType[] {
            new PlanetType(90, "Continental"),
            new PlanetType(0, "Barren"),
            new PlanetType(30, "Desert"),
            new PlanetType(150, "Paradise"),
            new PlanetType(50, "Ocean"),
            new PlanetType(0, "Gas giant"),
            new PlanetType(0, "Ice world"),
            new PlanetType(70, "Tropical"),
            new PlanetType(65, "Tundra")
        }; 

        static PlanetTypeContainer() {
            for (int i = 0; i < planetTypesArray.Length; i++) {
                planetTypes.Add((PlanetTypeValue)i, planetTypesArray[i]);
            }
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
        private bool isColonized = false;
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
            this.Name = name;

            if (radius < 2000) {
                throw new ArgumentOutOfRangeException(nameof(radius), "Can't be lower than 2000");
            }
            this.radius = radius;

            PlanetType planetType = PlanetTypeContainer.GetPlanetType(type);
            this.type = planetType;

            double planetArea = Math.Floor(HelperMathFunctions.SphereArea(this.radius));
            this.area = planetArea;

            this.maximumPopulation = (double)this.type.Quality * this.Area;

            this.buildingSites = (int)(this.MaximumPopulation / citizensPerSector);
            this.availableSites = this.buildingSites;

            if (this.MaximumPopulation > population) {
                this.Population = Math.Floor(population);
            }

            if(this.Population > 0) {
                this.IsColonized = true;
            }

            this.BodyResourse = SetPlanetResourses(planetType, planetArea);
        }

        private static Resourses SetPlanetResourses(PlanetType planetType, double planetArea) {
            double commonMetals = 0;
            double rareEarthElements = 0;
            double hydrogen = 0;

            if(planetType.Name != "Gas giant") {
                const double massOfTenKmCrust = (10d * ((3d) * 10E9));

                commonMetals = planetArea * (massOfTenKmCrust / 20);
                rareEarthElements = planetArea * (massOfTenKmCrust / 1E5);
                hydrogen = planetArea * (massOfTenKmCrust / 200);
            }

            Resourses planetResourses = new Resourses(hydrogen, commonMetals, rareEarthElements);

            return planetResourses;
        }

        /// <summary>
        /// Население планеты
        /// </summary>
        public double Population {
            get => this.population;
            private set {
                if (this.maximumPopulation > value && value > 0) {
                    this.population = value;
                    OnPropertyChanged();
                };
            }
        }

        public bool IsColonized {
            get => this.isColonized;
            private set {
                if (this.isColonized != value) {
                    this.isColonized = value;
                    OnPropertyChanged();
                }
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
            if (this.Population == 0) {
                return;
            }

            AddPopulation();
        
            CitizensToTheHub(player);
            CitizensFromTheHub(player);

            ExtractResourses(player);
        }

        private void AddPopulation() {
            double partOfGrowth = 0.0015d;

            double growthCoef = partOfGrowth * HelperRandomFunctions.GetRandomDouble();

            double addedPart =
                growthCoef * ((double)this.Type.Quality / (double)PlanetType.GoodWorldQuality);

            double addedPopulation = this.Population * addedPart;
            this.Population += addedPopulation;

            //return addedPopulation;
        }

        private void CitizensToTheHub(Player player) {
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
            if (this.BodyResourse > Resourses.Zero) {
                double minedResourses = this.Type.MiningDifficulty * this.Population;
                ExtractAllRseourses(player, minedResourses);
            }
        }

        private void ExtractAllRseourses(Player player, double miningCoef) {
            try {
                Resourses extracted = new Resourses(miningCoef, miningCoef, miningCoef);

                this.BodyResourse.Substract(extracted);
                player.OwnedResourses.Add(extracted);
            }
            catch (ArgumentException) {
                return;
            }
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
            if (player == null) {
                throw new ArgumentNullException(nameof(player));
            }

            if (this.Population > 0) {
                return true;
            }

            double colonizers = 10_000_000;

            if (this.MaximumPopulation > colonizers && player.GetColonizer()) {

                this.Population += colonizers;
                this.IsColonized = true;

                return true;
            }

            if (this.maximumPopulation > 0) {
                player.AddToColonizationQueue(this);
            }

            return false;
        }
    }
}
