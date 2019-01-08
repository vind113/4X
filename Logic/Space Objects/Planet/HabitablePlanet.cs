using Logic.PlayerClasses;
using Logic.PopulationClasses;
using Logic.Resource;
using Logic.SupportClasses;
using System;

namespace Logic.SpaceObjects {
    [Serializable]
    public class HabitablePlanet : Planet, IHabitablePlanet {

        private bool isColonized = false;

        public Population Population { get; }

        private const double citizensPerSector = 100_000_000d;

        public HabitablePlanet(string name, double radius, PlanetType type, long population) : base(name, radius, type) {
            this.Population = new Population(population, (double)this.Type.Quality * this.Area);

            this.BuildingSites = (int)Math.Ceiling(this.Population.MaxValue / citizensPerSector);
            this.AvailableSites = this.BuildingSites;

            if (this.Population.Value > 0) {
                this.IsColonized = true;
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
        public int BuildingSites { get; }

        /// <summary>
        /// Количество доступных для строительства площадок
        /// </summary>
        public int AvailableSites { get; }

        public override string ToString() {
            return $"{base.ToString()} Here lives {this.Population.Value:E4} intelligent creatures. " +
                $"On this planet can live {this.Population.MaxValue:E4} people. " +
                $"We can build {this.BuildingSites} buildings here. ";
        }

        public override void NextTurn(Player player) {
            base.NextTurn(player);

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
