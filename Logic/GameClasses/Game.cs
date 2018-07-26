using Logic.PlayerClasses;
using Logic.Space_Objects;
using Logic.SupportClasses;

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Logic.GameClasses {
    public class Game : INotifyPropertyChanged {
        private Player player;
        private CurrentDate gameDate;
        private bool isAutoColonizationEnabled = false;

        #region Events
        public event EventHandler<CitizenHubChangedEventArgs> CitizenHubChanged;
        public event EventHandler<StockpileChangedEventArgs> StockpileChanged;
        public event EventHandler<PopulationChangedEventArgs> PopulationChanged;
        
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private void OnCitizenHubChanged() {
            var handler = CitizenHubChanged;
            handler?.Invoke(this, new CitizenHubChangedEventArgs(player.PlayerCitizenHub)); 
        }

        private void OnStockpileChanged() {
            var handler = StockpileChanged;
            handler?.Invoke(this, new StockpileChangedEventArgs(player.Money, player.OwnedResourses));
        }

        private void OnPopulationChanged() {
            var handler = PopulationChanged;
            handler?.Invoke(this, new PopulationChangedEventArgs(player.TotalPopulation));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Game() {
            Player = new Player();
            GameDate = new CurrentDate();
            Player.StarSystems.Add(StarSystem.GetSolarSystem());
        }

        #region Properties
        public bool IsAutoColonizationEnabled {
            get => isAutoColonizationEnabled;
            set => isAutoColonizationEnabled = value;
        }

        public Player Player {
            get => this.player;
            set {
                if(this.Player == value) { return; }
                this.player = value;
                OnPropertyChanged();
            }
        }

        public CurrentDate GameDate {
            get => this.gameDate;
            set {
                this.gameDate = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Next Turn Functionality
        public void NextTurn() {
            GameDate = GameDate.NextTurn();
            DiscoverNewStarSystem();

            foreach (StarSystem system in Player.StarSystems) {
                system.NextTurn(Player);
            }
            SetPlayerCitizenHubCapacity();

            OnCitizenHubChanged();
            OnStockpileChanged();
            OnPopulationChanged();
        }

        private void DiscoverNewStarSystem() {
            //с такой вероятностью каждый ход будет открываться новая система
            //возможно добавить зависимость от уровня технологий
            //оптимальное значение - 0.15
            double discoveryProbability = 0.15; 
            
            if (HelperRandomFunctions.ProbableBool(discoveryProbability)) {
                int maxSystemsToGenerate = 0;
                int systemsToGenerate = 0;
                StarSystem generatedSystem = null;

                checked {
                    maxSystemsToGenerate = (int)((Math.Sqrt(Player.StarSystems.Count)) / 2);
                    systemsToGenerate = HelperRandomFunctions.GetRandomInt(1, maxSystemsToGenerate + 1);
                }

                for (int index = 0; index < systemsToGenerate; index++) {
                    generatedSystem = StarSystemFactory.GenerateSystem($"System {GameDate.Date} #{index}");

                    Player.OwnedStars += generatedSystem.SystemStars.Count;
                    Player.OwnedPlanets += generatedSystem.SystemPlanets.Count;

                    if (isAutoColonizationEnabled) {
                        foreach (var planet in generatedSystem.SystemPlanets) {
                            planet.Colonize(Player);
                        }
                    }

                    Player.StarSystems.Add(generatedSystem);
                }
            }
        }

        private void SetPlayerCitizenHubCapacity() {
            double newHubCapacity = Math.Ceiling(Player.TotalPopulation / 1000);
            
            if (newHubCapacity > Player.PlayerCitizenHub.CitizensInHub) {
                Player.PlayerCitizenHub.MaximumCount = newHubCapacity;
            }
        }

        #endregion
    }
}
