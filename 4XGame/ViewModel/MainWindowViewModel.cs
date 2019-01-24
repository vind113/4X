using _4XGame.Serialization;
using _4XGame.ViewModel.Commands;
using Logic.GameClasses;
using Logic.PlayerClasses;
using Logic.Resource;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace _4XGame.ViewModel {
    public class MainWindowViewModel : INotifyPropertyChanged {
        private Game currentGame;

        private double money;
        private double totalPopulation;

        private IMutableResources resources;
        public BodiesCount BodiesCount { get; } = new BodiesCount();

        private int сolonizedCount;

        private string gameEventsLog;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel() {
            //HelperRandomFunctions.SetToInitialSeed(1);

            GameEventsLog = String.Empty;

            SetNewGame(new Game());
        }

        private void SetNewGame(Game game) {
            this.CurrentGame = game;

            SetPlayerHandlers(CurrentGame.Player);
            SetViewModelProperties(CurrentGame.Player);
        }

        private void SetViewModelProperties(Player player) {
            this.CurrentResources = player.OwnedResources;
            this.Money = player.Money;
            this.TotalPopulation = player.Population;
            this.BodiesCount.SetBodiesCount(player.StarSystemsCount, player.OwnedStars, player.OwnedPlanets);
            this.СolonizedCount = player.ColonizedPlanets;
        }

        private void SetPlayerHandlers(Player player) {
            player.StockpileChanged += SetStockpile;
            player.PropertyChanged += this.Player_PropertyChanged;
            SetEmpireEventHandlers(player.Empire);
        }

        private void SetEmpireEventHandlers(Empire empire) {
            empire.PopulationChanged += this.Empire_PopulationChanged;
            SetContainerHandlers(empire.Container);
        }

        private void SetContainerHandlers(StarSystemContainer container) {
            container.ColonizedCountChanged += this.Container_ColonizedCountChanged;
            container.BodiesCountChanged += this.Container_BodiesCountChanged;
        }

        private void Container_ColonizedCountChanged(object sender, EventArgs e) {
            this.СolonizedCount = CurrentGame.Player.ColonizedPlanets;
        }

        private void Empire_PopulationChanged(object sender, EventArgs e) {
            this.TotalPopulation = CurrentGame.Player.Population;
        }

        private void Player_PropertyChanged(object sender, PropertyChangedEventArgs e) {

        }

        private void Container_BodiesCountChanged(object sender, EventArgs e) {
            this.BodiesCount.SetBodiesCount(
                CurrentGame.Player.StarSystemsCount, CurrentGame.Player.OwnedStars, CurrentGame.Player.OwnedPlanets);
        }

        private void SetStockpile(object sender, StockpileChangedEventArgs e) {
            Money = e.Money;
            CurrentResources = e.ArgResources;
        }

        #region Commands
        [NonSerialized]
        private NextTurnCommand nextTurnCommand = null;
        public NextTurnCommand NextTurnCmd => nextTurnCommand ?? (nextTurnCommand = new NextTurnCommand());

        [NonSerialized]
        private MultipleNextTurnsCommand multipleNextTurnsCommand = null;
        public MultipleNextTurnsCommand MultipleNextTurnsCmd =>
            multipleNextTurnsCommand ?? (multipleNextTurnsCommand = new MultipleNextTurnsCommand());

        [NonSerialized]
        private ColonizePlanetCommand colonizePlanetCommand = null;
        public ColonizePlanetCommand ColonizePlanetCmd =>
            colonizePlanetCommand ?? (colonizePlanetCommand = new ColonizePlanetCommand());

        [NonSerialized]
        private BuildHabitatCommand buildHabitatCommand = null;
        public BuildHabitatCommand BuildHabitatCmd =>
            buildHabitatCommand ?? (buildHabitatCommand = new BuildHabitatCommand());

        [NonSerialized]
        private RelayCommand<MainWindow> newGameCommand = null;
        public RelayCommand<MainWindow> NewGameCmd =>
            newGameCommand ?? (newGameCommand = new RelayCommand<MainWindow>(
                (o) => {
                    this.SetNewGame(new Game());
                }));

        [NonSerialized]
        private RelayCommand<MainWindow> saveGameCommand = null;
        public RelayCommand<MainWindow> SaveGameCmd =>
            saveGameCommand ?? (saveGameCommand = new RelayCommand<MainWindow>(
                (o) => {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.Filter = "save files (*.dat)|*.dat";
                    if (saveDialog.ShowDialog() == true) {
                        GameSaveLoad.Save(this.CurrentGame, saveDialog.FileName);
                    }
                }));

        [NonSerialized]
        private RelayCommand<MainWindow> loadGameCommand = null;
        public RelayCommand<MainWindow> LoadGameCmd =>
            loadGameCommand ?? (loadGameCommand = new RelayCommand<MainWindow>(
                (o) => {
                    try {
                        OpenFileDialog chooseSaveGameDialog = new OpenFileDialog();
                        chooseSaveGameDialog.Filter = "save files (*.dat)|*.dat|All files (*.*)|*.*";
                        if (chooseSaveGameDialog.ShowDialog() == true) {
                            this.SetNewGame(GameSaveLoad.Load(chooseSaveGameDialog.FileName));
                        }
                    }
                    catch (SaveFileException ex) {
                        MessageBox.Show(ex.Message, "Cannot load game");
                    }
                }));
        #endregion

        #region Properties
        public Game CurrentGame {
            get => this.currentGame;
            private set {
                this.currentGame = value;
                OnPropertyChanged();
            }
        }

        public double Money {
            get => this.money;
            set {
                if (this.Money == value) { return; }
                this.money = value;
                OnPropertyChanged();
            }
        }

        public IMutableResources CurrentResources {
            get => this.resources;
            set {
                this.resources = value;
                OnPropertyChanged();
            }
        }

        public double TotalPopulation {
            get => this.totalPopulation;
            set {
                this.totalPopulation = value;
                OnPropertyChanged();
            }
        }

        public string GameEventsLog {
            get => this.gameEventsLog;
            set {
                this.gameEventsLog = value;
                OnPropertyChanged();
            }
        }

        public int СolonizedCount {
            get => this.сolonizedCount;
            private set {
                this.сolonizedCount = value;
                OnPropertyChanged();
            }
        }
        #endregion
    }
}
