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
        private byte resourseAbundance;

        public PlanetType(byte quality, string name, byte resourseAbundance = 100) {
            this.quality = quality;
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.resourseAbundance = resourseAbundance;

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

    public enum PlanetTypeVariants {
        Continental, Barren, Desert, Paradise, Ocean, GasGiant, IceWorld, Tropical, Tundra
    }

    static public class PlanetTypeContainer {
        static Dictionary<PlanetTypeVariants, PlanetType> planetTypes = new Dictionary<PlanetTypeVariants, PlanetType>();
        static PlanetType[] planetTypesArray = new PlanetType[] {
            new PlanetType(90, "Continental"),
            new PlanetType(0, "Barren"),
            new PlanetType(10, "Desert"),
            new PlanetType(150, "Paradise"),
            new PlanetType(40, "Ocean"),
            new PlanetType(0, "Gas giant"),
            new PlanetType(0, "Ice world"),
            new PlanetType(40, "Tropical"),
            new PlanetType(50, "Tundra")
        }; 

        static PlanetTypeContainer() {
            for (int i = 0; i < planetTypesArray.Length; i++) {
                planetTypes.Add((PlanetTypeVariants)i, planetTypesArray[i]);
            }
        }

        public static PlanetType GetPlanetType(PlanetTypeVariants key) {
            if (planetTypes.ContainsKey(key)) {
                return planetTypes[key];
            }
            else {
                throw new ArgumentException("Passed key does not have a value");
            }
        }
    }

    /// <summary>
    /// Представляет планету
    /// </summary>
    [Serializable]
    public class Planet : CelestialBody, IHabitable {
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
        public Planet(string name, double radius, PlanetTypeVariants type, double population) {
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

            this.buildingSites = (int)Math.Ceiling(this.MaximumPopulation / citizensPerSector);
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
                double massOfTenKmCrust = (10d * ((3d) * 10E9));

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
            set {
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

            AddPopulation(player.PopulationGrowthModifier);

            player.Hub.MigrateHabitatToHub(this);
            player.Hub.MigrateHubToHabitat(this);

            ExtractResourses(player);
        }

        private void AddPopulation(double partOfGrowth) {
            double growthCoef = partOfGrowth * HelperRandomFunctions.GetRandomDouble();

            double addedPart =
                growthCoef * ((double)this.Type.Quality / (double)PlanetType.GoodWorldQuality);

            double addedPopulation = this.Population * addedPart;
            this.Population += addedPopulation;
        }

        private void ExtractResourses(Player player) {
            if (this.BodyResourse.IsStrictlyGreater(Resourses.Zero)) {
                double minedResourses = (this.Type.MiningDifficulty * this.Population);

                double minedHydrogen = minedResourses / 10;
                double minedCommonMetals = minedResourses;
                double minedRareElements = minedResourses / 5_000;
                try {
                    Resourses extracted = new Resourses(minedHydrogen, minedCommonMetals, minedRareElements);

                    this.BodyResourse.Substract(extracted);
                    player.OwnedResourses.Add(extracted);
                }
                catch (ArgumentException) {
                    player.OwnedResourses.Add(this.BodyResourse);
                    this.BodyResourse.SetToZero();
                }
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
            if (this.Population > 0) {
                return true;
            }

            if (this.MaximumPopulation < Colonizer.Colonists) {
                return false;
            }

            if (player == null) {
                throw new ArgumentNullException(nameof(player));
            }

            Colonizer colonizer = Ships.GetColonizerFrom(player.OwnedResourses); 

            if (colonizer != null) {

                this.Population += colonizer.GetColonists(colonizer.ColonistsOnShip);
                this.IsColonized = true;

                return true;

            }
            else {
                player.AddToColonizationQueue(this);
                return false;
            }
        }
    }
}
