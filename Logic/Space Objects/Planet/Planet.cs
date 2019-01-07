using System;
using Logic.SupportClasses;
using Logic.PlayerClasses;
using Logic.Resourse;
using Logic.PopulationClasses;

namespace Logic.SpaceObjects {
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

            this.buildingSites = (int)Math.Ceiling(this.Population.MaxValue / citizensPerSector);
            this.availableSites = this.buildingSites;

            if (this.Population.Value > 0) {
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

        /// Возвращает тип планеты
        /// </summary>
        public PlanetType Type { get => this.type; }

        public Population Population { get => this.population; }

        public override string ToString() {
            return $"{this.Name} is a {this.Type.Name} world with radius of {this.Radius} km " +
                $"and area of {this.Area:E4} km^2. " +
                $"Here lives {this.Population.Value:E4} intelligent creatures. " +
                $"On this planet can live {this.Population.MaxValue:E4} people. " +
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
            if (this.Population.Value == 0) {
                return;
            }

            AddPopulation(player.PopulationGrowthFactor);

            ConductMigration(player.Hub);

            ExtractResourses(player.OwnedResourses);
        }

        private void ConductMigration(CitizenHub migrationHub) {
            migrationHub.MigrateToHub(this.Population);
            migrationHub.MigrateFromHub(this.Population);
        }

        private void AddPopulation(double growthFactor) {
            double growthCoef = growthFactor * HelperRandomFunctions.GetRandomDouble();

            double addedPart =
                growthCoef * (this.Type.Quality / PlanetType.GoodWorldQuality);

            double addedPopulation = this.Population.Value * addedPart;

            this.population.Add(addedPopulation);
        }

        private void ExtractResourses(Resourses extractTo) {
            if (this.BodyResourse.IsStrictlyGreater(Resourses.Zero)) {
                double minedResourses = (this.Type.MiningDifficulty * this.Population.Value);

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
            if (this.Population.Value > 0) {
                return ColonizationState.Colonized;
            }

            if (this.Population.MaxValue < Colonizer.Colonists) {
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
