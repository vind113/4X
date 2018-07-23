﻿using _4XGame.ViewModel.Commands;
using Logic.GameClasses;
using Logic.Resourse;
using Logic.Space_Objects;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace _4XGame.ViewModel {
    public class MainWindowViewModel : INotifyPropertyChanged {
        private Game thisGame;

        private double money;
        private double citizensInHub;
        private double totalPopulation;

        private Resourses resourses;
        private Planet selectedPlanet;
        private Star selectedStar;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindowViewModel() {
            ThisGame = new Game();

            this.resourses = new Resourses(0, 0, 0);

            ThisGame.CitizenHubChanged += SetCitizensInHub;
            ThisGame.StockpileChanged += SetStockpile;
            ThisGame.PopulationChanged += SetTotalPopulation;
        }

        #region Commands
        private NextTurnCommand nextTurnCommand = null;
        public NextTurnCommand NextTurnCmd => nextTurnCommand ?? (nextTurnCommand = new NextTurnCommand());

        private MultipleNextTurnsCommand multipleNextTurnsCommand = null;
        public MultipleNextTurnsCommand MultipleNextTurnsCmd =>
            multipleNextTurnsCommand ?? (multipleNextTurnsCommand = new MultipleNextTurnsCommand());
        #endregion

        #region Properties
        public Game ThisGame {
            get => this.thisGame;
            set {
                this.thisGame = value;
                OnPropertyChanged();
            }
        }

        public double CitizensInHub {
            get => this.citizensInHub;
            set {
                if (this.CitizensInHub == value) { return; }
                this.citizensInHub = value;
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
                //добавь проверку
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

        public Planet SelectedPlanet {
            get => this.selectedPlanet;
            set {
                this.selectedPlanet = value;
                OnPropertyChanged();
            }
        }

        public Star SelectedStar {
            get => this.selectedStar;
            set {
                this.selectedStar = value;
                OnPropertyChanged();
            }
        }
        #endregion

        private void SetCitizensInHub(object sender, CitizenHubChangedEventArgs e) {
            CitizensInHub = e.CitizensInHub;   
        }

        private void SetStockpile(object sender, StockpileChangedEventArgs e) {
            Money = e.Money;
            CurrentResourses = e.ArgResourses;
        }

        private void SetTotalPopulation(object sender, PopulationChangedEventArgs e) {
            TotalPopulation = e.Population;
        }
    }
}
