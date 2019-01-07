using System;
using Logic.SupportClasses;
using Logic.PlayerClasses;
using Logic.Resource;
using Logic.PopulationClasses;
using Logic.SpaceObjects.PlanetClasses;

namespace Logic.SpaceObjects {
    /// <summary>
    /// Представляет планету
    /// </summary>
    [Serializable]
    public class Planet : CelestialBody, IHabitable, IPlanet {

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

            this.Type = type;

            this.Population = new Population(population, (double)this.Type.Quality * this.Area);

            this.BuildingSites = (int)Math.Ceiling(this.Population.MaxValue / citizensPerSector);
            this.AvailableSites = this.BuildingSites;

            if (this.Population.Value > 0) {
                this.IsColonized = true;
            }

            this.BodyResource = new PlanetResourceGenerator().GenerateFor(this);
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
        public int BuildingSites { get; }

        /// <summary>
        /// Количество доступных для строительства площадок
        /// </summary>
        public int AvailableSites { get; }

        /// Возвращает тип планеты
        /// </summary>
        public PlanetType Type { get; }

        public Population Population { get; }

        public override string ToString() {
            return $"{this.Name} is a {this.Type.Name} world with radius of {this.Radius} km " +
                $"and area of {this.Area:E4} km^2. " +
                $"Here lives {this.Population.Value:E4} intelligent creatures. " +
                $"On this planet can live {this.Population.MaxValue:E4} people. " +
                $"We can build {this.BuildingSites} buildings here. ";
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

            ExtractResources(player.OwnedResources);
        }

        private void ConductMigration(CitizenHub migrationHub) {
            migrationHub.MigrateToHub(this.Population);
            migrationHub.MigrateFromHub(this.Population);
        }

        private void AddPopulation(double growthFactor) {
            double growthCoef = growthFactor * HelperRandomFunctions.GetRandomDouble();

            double addedPart =
                growthCoef * (this.Type.Quality / PlanetType.GOOD_WORLD_QUALITY);

            double addedPopulation = this.Population.Value * addedPart;

            this.Population.Add(addedPopulation);
        }

        private void ExtractResources(Resources extractTo) {
            if (this.BodyResource.IsStrictlyGreater(Resources.Zero)) {
                double minedResources = (this.Type.MiningDifficulty * this.Population.Value);

                double minedHydrogen = minedResources / 10;
                double minedCommonMetals = minedResources;
                double minedRareElements = minedResources / 5_000;
                try {
                    Resources extracted = new Resources(minedHydrogen, minedCommonMetals, minedRareElements);

                    this.BodyResource.Subtract(extracted);
                    extractTo.Add(extracted);
                }
                catch (ArgumentException) {
                    extractTo.Add(this.BodyResource);
                    this.BodyResource.SetToZero();
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

            Colonizer colonizer = player.Ships.GetColonizer(player.OwnedResources);

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
