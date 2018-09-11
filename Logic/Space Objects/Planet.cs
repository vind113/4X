﻿using System;
using Logic.SupportClasses;
using Logic.PlayerClasses;
using Logic.Resourse;
using Logic.PopulationClasses;

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

    public enum ColonizationState : byte {
        Unknown, NonColonizable, NotColonized, Colonized
    }


    /// <summary>
    /// Представляет планету
    /// </summary>
    [Serializable]
    public class Planet : CelestialBody, IHabitable {
        private PlanetType type;            
                
        private readonly int buildingSites;          
        private int availableSites;

        private Population population;
        private bool isColonized = false;

        private const double citizensPerSector = 100_000_000d;

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
        ///     Тип планеты
        /// </param>
        /// <param name="population">
        ///     Изначальное население планеты
        /// </param>
        public Planet(string name, double radius, PlanetType type, long population):base(name, radius) {
            if (radius < 2000) {
                throw new ArgumentOutOfRangeException(nameof(radius), "Cannot be lower than 2000");
            }

            this.type = type;

            this.population = new Population(population, (double)this.type.Quality * this.Area);

            this.buildingSites = (int)Math.Ceiling(this.MaximumPopulation / citizensPerSector);
            this.availableSites = this.buildingSites;

            if (this.PopulationValue > 0) {
                this.IsColonized = true;
            }

            this.SetPlanetResourses();
        }

        private void SetPlanetResourses() {
            double commonMetals = 0;
            double rareEarthElements = 0;
            double hydrogen = 0;

            double massOfTenKmCrust = (10d * ((3d) * 10E9));

            if (this.Type.SubstancesClass == SubstancesClass.Terra) {
                commonMetals = this.Area * (massOfTenKmCrust / 20);
                rareEarthElements = this.Area * (massOfTenKmCrust / 1E5);
                hydrogen = this.Area * (massOfTenKmCrust / 200);

            }
            else if(this.Type.SubstancesClass == SubstancesClass.Ferria) {
                commonMetals = this.Area * (massOfTenKmCrust / 2);
                rareEarthElements = this.Area * (massOfTenKmCrust / 1E4);
                hydrogen = this.Area * (massOfTenKmCrust / 200);

            }
            else if(this.Type.SubstancesClass == SubstancesClass.Jupiter) {
                commonMetals = 0;
                rareEarthElements = 0;
                hydrogen = this.Area * (1.4 * 20 * 1E3 * 1E9);

            }

            this.BodyResourse = new Resourses(hydrogen, commonMetals, rareEarthElements);
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
        /// Население планеты
        /// </summary>
        public long PopulationValue { get => this.population.Value; }

        /// <summary>
        /// Максимально возможное количество жителей на планете
        /// </summary>
        public long MaximumPopulation { get => this.population.MaxValue; }

        /// <summary>
        /// Возвращает тип планеты
        /// </summary>
        public PlanetType Type { get => this.type; }

        public Population Population { get => this.population; }

        public override string ToString() {
            return $"{this.Name} is a {this.Type.Name} world with radius of {this.Radius} km " +
                $"and area of {this.Area:E4} km^2. " +
                $"Here lives {this.PopulationValue:E4} intelligent creatures. " +
                $"On this planet can live {this.MaximumPopulation:E4} people. " +
                $"We can build {this.buildingSites} buildings here. ";
        }
    
        #region Next turn methods
        /// <summary>
        ///     Выполняет все операции для перехода на следующий ход
        /// </summary>
        /// <param name="player">
        ///     Игрок, которому принадлежит планета
        /// </param>
        public void NextTurn(Player player) {
            if (this.PopulationValue == 0) {
                return;
            }

            AddPopulation(player.PopulationGrowthFactor);

            player.Hub.MigrateToHub(this);
            player.Hub.MigrateFromHub(this);

            ExtractResourses(player.OwnedResourses);
        }

        private void AddPopulation(double partOfGrowth) {
            double growthCoef = partOfGrowth * HelperRandomFunctions.GetRandomDouble();

            double addedPart =
                growthCoef * (this.Type.Quality / PlanetType.GoodWorldQuality);

            double addedPopulation = this.PopulationValue * addedPart;

            this.population.Add(addedPopulation);
        }

        private void ExtractResourses(Resourses extractTo) {
            if (this.BodyResourse.IsStrictlyGreater(Resourses.Zero)) {
                double minedResourses = (this.Type.MiningDifficulty * this.PopulationValue);

                double minedHydrogen = minedResourses / 10;
                double minedCommonMetals = minedResourses;
                double minedRareElements = minedResourses / 5_000;
                try {
                    Resourses extracted = new Resourses(minedHydrogen, minedCommonMetals, minedRareElements);

                    this.BodyResourse.Substract(extracted);
                    extractTo.Add(extracted);
                }
                catch (ArgumentException) {
                    extractTo.Add(this.BodyResourse);
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
        public ColonizationState Colonize(Player player) {
            if (this.PopulationValue > 0) {
                return ColonizationState.Colonized;
            }

            if (this.MaximumPopulation < Colonizer.Colonists) {
                return ColonizationState.NonColonizable;
            }

            if (player == null) {
                throw new ArgumentNullException(nameof(player));
            }

            Colonizer colonizer = player.Ships.GetColonizer(player.OwnedResourses);

            if (colonizer != null) {

                this.Population.Add(colonizer.GetColonists(colonizer.ColonistsOnShip));
                this.IsColonized = true;

                return ColonizationState.Colonized;

            }
            else {
                player.AddToColonizationQueue(this);
                return ColonizationState.NotColonized;
            }
        }
    }
}
