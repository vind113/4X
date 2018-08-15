using System;
using Logic.SupportClasses;
using Logic.PlayerClasses;
using System.Collections.Generic;
using Logic.Resourse;

namespace Logic.SpaceObjects {
    //from Space Engine
    public enum TemperatureClass : byte {
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

    public enum VolatilesClass : byte {
        Airless, Desertic, Lacustrine, Marine, Oceanic, Superoceanic
    }

    public enum SubstancesClass : byte {
        Ferria, Terra, Jupiter
    }

    public class PlanetCharacteristicsHelper {

        public static double GetPlanetTypeFactor(
            TemperatureClass temperature, VolatilesClass volatiles, SubstancesClass substances) {

            return (GetTemperatureFactor(temperature) * GetVolatilesFactor(volatiles) * GetSubstancesFactor(substances));
        }

        private static double GetTemperatureFactor(TemperatureClass temperatureClass) {
            switch (temperatureClass) {
                case (TemperatureClass.Frigid):
                    return 0;
                case (TemperatureClass.Cold):
                    return 0.01;
                case (TemperatureClass.Cool):
                    return 0.9;
                case (TemperatureClass.Temperate):
                    return 1.1;
                case (TemperatureClass.Warm):
                    return 0.01;
                case (TemperatureClass.Hot):
                    return 0;
                default:
                    throw new ArgumentException($"{temperatureClass.ToString()} class is not acceptable");
            }
        }

        private static double GetVolatilesFactor(VolatilesClass volatilesClass) {
            switch (volatilesClass) {
                case (VolatilesClass.Airless):
                    return 0;
                case (VolatilesClass.Desertic):
                    return 0.01;
                case (VolatilesClass.Lacustrine):
                    return 0.05;
                case (VolatilesClass.Marine):
                    return 1.2;
                case (VolatilesClass.Oceanic):
                    return 0.3;
                case (VolatilesClass.Superoceanic):
                    return 0.1;
                default:
                    throw new ArgumentException($"{volatilesClass.ToString()} class is not acceptable");
            }
        }

        private static double GetSubstancesFactor(SubstancesClass substancesClass) {
            switch (substancesClass) {
                case (SubstancesClass.Ferria):
                    return 0.7;
                case (SubstancesClass.Terra):
                    return 1;
                case (SubstancesClass.Jupiter):
                    return 0;
                default:
                    throw new ArgumentException($"{substancesClass.ToString()} class is not acceptable");
            }
        }
    }

    [Serializable]
    public struct PlanetType {
        private const double goodWorldQuality = 100;

        private double quality;
        private string name;
        private double miningDifficulty;
        private byte resourseAbundance;

        private TemperatureClass temperatureClass;
        private VolatilesClass volatilesClass;
        private SubstancesClass substancesClass;

        public PlanetType(TemperatureClass temperature, VolatilesClass volatiles, SubstancesClass substances) {
            this.temperatureClass = temperature;
            this.volatilesClass = volatiles;
            this.substancesClass = substances;

            double tempQuality = PlanetCharacteristicsHelper.GetPlanetTypeFactor(temperature,volatiles,substances);

            this.quality = tempQuality * GoodWorldQuality;
            this.name = $"{temperature}, {volatiles}, {substances}";
            this.resourseAbundance = 100;

            double months = 12;
            this.miningDifficulty = (quality / PlanetType.GoodWorldQuality) / months;
        }

        public double Quality { get => this.quality; }
        public string Name { get => this.name; }

        public static double GoodWorldQuality { get => goodWorldQuality; }
        public double MiningDifficulty { get => this.miningDifficulty; }
        public byte ResourseAbundance { get => this.resourseAbundance; }

        public TemperatureClass TemperatureClass { get => this.temperatureClass; }
        public VolatilesClass VolatilesClass { get => this.volatilesClass; }
        public SubstancesClass SubstancesClass { get => this.substancesClass; }
    }

    public enum PlanetTypeVariants {
        Terra, Barren, Desert, Paradise, GasGiant
    }

    static public class PlanetTypeContainer {
        static Dictionary<PlanetTypeVariants, PlanetType> planetTypes = new Dictionary<PlanetTypeVariants, PlanetType>();
        static PlanetType[] planetTypesArray = new PlanetType[] {
            new PlanetType(TemperatureClass.Cool, VolatilesClass.Marine, SubstancesClass.Terra),
            new PlanetType(TemperatureClass.Cold, VolatilesClass.Airless, SubstancesClass.Terra),
            new PlanetType(TemperatureClass.Cool, VolatilesClass.Desertic, SubstancesClass.Terra),
            new PlanetType(TemperatureClass.Temperate, VolatilesClass.Marine, SubstancesClass.Terra),
            new PlanetType(TemperatureClass.Cold, VolatilesClass.Airless, SubstancesClass.Jupiter)
        }; 

        static PlanetTypeContainer() {
            /*
            for (int i = 0; i < planetTypesArray.Length; i++) {
                planetTypes.Add((PlanetTypeVariants)i, planetTypesArray[i]);
            }
            */
            
            planetTypes.Add(PlanetTypeVariants.Terra, new PlanetType(TemperatureClass.Cool, VolatilesClass.Marine, SubstancesClass.Terra));
            planetTypes.Add(PlanetTypeVariants.Barren, new PlanetType(TemperatureClass.Cold, VolatilesClass.Airless, SubstancesClass.Terra));
            planetTypes.Add(PlanetTypeVariants.Desert, new PlanetType(TemperatureClass.Cool, VolatilesClass.Desertic, SubstancesClass.Terra));
            planetTypes.Add(PlanetTypeVariants.Paradise, new PlanetType(TemperatureClass.Temperate, VolatilesClass.Marine, SubstancesClass.Terra));
            planetTypes.Add(PlanetTypeVariants.GasGiant, new PlanetType(TemperatureClass.Cold, VolatilesClass.Airless, SubstancesClass.Jupiter));
            
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
        public Planet(string name, double radius, PlanetTypeVariants type, double population):
                      this(name, radius, PlanetTypeContainer.GetPlanetType(type), population){

        }

        public Planet(string name, double radius, PlanetType type, double population) {
            if (radius < 2000) {
                throw new ArgumentOutOfRangeException(nameof(radius), "Cannnot be lower than 2000");
            }

            this.type = type;

            this.Name = name;

            this.radius = radius;

            double planetArea = Math.Floor(HelperMathFunctions.SphereArea(this.radius));
            this.area = planetArea;

            this.maximumPopulation = (double)this.type.Quality * this.Area;

            this.buildingSites = (int)Math.Ceiling(this.MaximumPopulation / citizensPerSector);
            this.availableSites = this.buildingSites;

            if (this.MaximumPopulation > population) {
                this.Population = Math.Floor(population);
            }

            if (this.Population > 0) {
                this.IsColonized = true;
            }

            this.BodyResourse = SetPlanetResourses(type, planetArea);
        }

        private static Resourses SetPlanetResourses(PlanetType planetType, double planetArea) {
            double commonMetals = 0;
            double rareEarthElements = 0;
            double hydrogen = 0;

            double massOfTenKmCrust = (10d * ((3d) * 10E9));

            if (planetType.SubstancesClass == SubstancesClass.Terra) {
                commonMetals = planetArea * (massOfTenKmCrust / 20);
                rareEarthElements = planetArea * (massOfTenKmCrust / 1E5);
                hydrogen = planetArea * (massOfTenKmCrust / 200);
            }
            else if(planetType.SubstancesClass == SubstancesClass.Ferria) {
                commonMetals = planetArea * (massOfTenKmCrust / 2);
                rareEarthElements = planetArea * (massOfTenKmCrust / 1E4);
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

            AddPopulation(player.PopulationGrowthFactor);

            player.Hub.MigrateHabitatToHub(this);
            player.Hub.MigrateHubToHabitat(this);

            ExtractResourses(player);
        }

        private void AddPopulation(double partOfGrowth) {
            double growthCoef = partOfGrowth * HelperRandomFunctions.GetRandomDouble();

            double addedPart =
                growthCoef * (this.Type.Quality / PlanetType.GoodWorldQuality);

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
