using _4XGame.Serialization;
using _4XGame.ViewModel.Commands;
using Logic.GameClasses;
using Logic.Resourse;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace _4XGame.ViewModel {
    [Serializable]
    public class MainWindowViewModel : INotifyPropertyChanged {
        private Game currentGame;

        private double money;
        private double totalPopulation;

        private Resourses resourses;

        private string gameEventsLog;

        [field: NonSerialized]
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

            CurrentGame.Player.StockpileChanged += SetStockpile;
            CurrentGame.Player.PopulationChanged += SetTotalPopulation;

            this.CurrentResourses = this.CurrentGame.Player.OwnedResourses;
            this.Money = this.CurrentGame.Player.Money;
            this.TotalPopulation = this.CurrentGame.Player.TotalPopulation;
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
            set {
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

        public Resourses CurrentResourses {
            get => this.resourses;
            set {
                this.resourses = value;
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
        #endregion

        private void SetStockpile(object sender, StockpileChangedEventArgs e) {
            Money = e.Money;
            CurrentResourses = e.ArgResourses;
        }

        private void SetTotalPopulation(object sender, PopulationChangedEventArgs e) {
            TotalPopulation = e.Population;
        }
    }
}
