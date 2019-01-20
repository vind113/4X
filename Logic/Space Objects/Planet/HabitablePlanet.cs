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
            double maximumPopulation = (double)this.Type.Quality * this.Area;
            this.Population = new Population(population, maximumPopulation);

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
                    OnPropertyChanged(nameof(HabitablePlanet.Population.Value));
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

            AddPopulation(Player.PopulationGrowthFactor * HelperRandomFunctions.GetRandomDouble());
            ConductMigration(player.Hub);
            ExtractResources(player.OwnedResources);
        }

        private void AddPopulation(double growthFactor) {
            double growthPart = growthFactor * (this.Type.Quality / PlanetType.GoodWorldQuality);
            double addedPopulation = this.Population.Value * growthPart;

            this.Population.Add(addedPopulation);
        }

        private void ConductMigration(CitizenHub migrationHub) {
            migrationHub.ConductMigration(this.Population);
        }

        private void ExtractResources(IMutableResources extractTo) {
            if (this.BodyResource.IsStrictlyGreater(Resources.Zero)) {
                double minedResources = (this.Type.MiningDifficulty * this.Population.Value);

                double minedHydrogen = minedResources / 10;
                double minedCommonMetals = minedResources;
                double minedRareElements = minedResources / 5_000;
                try {
                    IBasicResources extracted = new Resources(minedHydrogen, minedCommonMetals, minedRareElements);

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
        ///     Значение <see cref="ColonizationState"/>, которое показывает статус колонизации
        /// </returns>
        public ColonizationState Colonize(Colonizer colonizer) {
            if (this.Population.Value > 0) {
                return ColonizationState.Colonized;
            }

            if (colonizer != null && this.Population.MaxValue >= Colonizer.Colonists) {
                this.Population.Add(colonizer.GetColonists(colonizer.ColonistsOnShip));
                this.IsColonized = true;

                return ColonizationState.Colonized;
            }
            else {
                return ColonizationState.NotColonized;
            }
        }
    }
}
