﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Logic.GameClasses;
using Logic.PlayerClasses;
using Logic.Space_Objects;
using Logic.SupportClasses;

namespace _4XGame {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            SetItemsSource();

            RefreshGUI();
        }

        private void SetItemsSource() {
            PlayerPlanetsTree.ItemsSource = Game.Player.StarSystems;
            PlayerStarsTree.ItemsSource = Game.Player.StarSystems;
        }

        private void NextTurnButton_Click(object sender, RoutedEventArgs e) {
            Game.NextTurn();
            RefreshGUI();
        }

        private void RefreshGUI() {
            //сворачивает вкладку, исправь
            //PlayerStarsTree.Items.Refresh();
            //PlayerPlanetsTree.Items.Refresh();

            WriteStarInfoToBox();
            WritePlanetInfoToBox();

            ShowPlayerMoney.Content = $"{Game.Player.PlayerMoney:0.0000E0}";
            ShowCitizenHub.Content = $"{Game.Player.PlayerCitizenHub.CitizensInHub:0.0000E0}";
            //ShowCitizenHub.Content = $"{Game.Player.PlayerCitizenHub.MaximumCount:0.0000E0}";
            ShowPlayerTotalPopulation.Content = $"{Game.Player.TotalPopulation:0.0000E0}";

            ShowPlayerHydrogen.Content = $"{Game.Player.PlayerResourses.Hydrogen:0.0000E0}";
            ShowPlayerMetals.Content = $"{Game.Player.PlayerResourses.CommonMetals:0.0000E0}";
            ShowPlayerRareMetals.Content = $"{Game.Player.PlayerResourses.RareEarthElements:0.0000E0}";

            RefreshColonizedPlanetsValueLabel();
            RefreshOwnedPlanetsValueLabel();
            RefreshOwnedStarsValueLabel();

            ShowTurnLabel.Content = Game.GameTurn;
            ShowDateLabel.Content = Game.GameDate;
        }

        private void PlayerStarsTree_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            WriteStarInfoToBox();
        }

        private void WriteStarInfoToBox() {
            if (PlayerStarsTree.SelectedItem != null && PlayerStarsTree.SelectedItem is Star star) {
                StarNameValue.Content = star.Name;
                StarRadiusValue.Content = $"{star.Radius} km";
                StarAreaValue.Content = $"{star.Area:E4} km^2";
                StarTypeValue.Content = star.LumClass;
            }
        }

        private void PlayerPlanetsTree_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            WritePlanetInfoToBox();
        }

        private void WritePlanetInfoToBox() {
            if (PlayerPlanetsTree.SelectedItem != null && PlayerPlanetsTree.SelectedItem is Planet planet) {
                PlanetNameValue.Content = planet.Name;
                PlanetRadiusValue.Content = $"{planet.Radius} km";
                PlanetAreaValue.Content = $"{planet.Area:E4} km^2";
                PlanetPopulationValue.Content = $"{planet.Population:E5}";
                PlanetTypeValue.Content = planet.Type.Name;
            }
        }

        private void ColonizePlanet_Click(object sender, RoutedEventArgs e) {
            if (PlayerPlanetsTree.SelectedItem != null && PlayerPlanetsTree.SelectedItem is Planet planet) {
                planet.Colonize(Game.Player);
                RefreshColonizedPlanetsValueLabel();
            }
        }

        private void RefreshColonizedPlanetsValueLabel() {
            ColonizedPlanetsValue.Content = Game.Player.ColonizedPlanets;
        }

        private void RefreshOwnedPlanetsValueLabel() {
            OwnedPlanetsValue.Content = Game.Player.OwnedPlanets;
        }

        private void RefreshOwnedStarsValueLabel() {
            OwnedStarsValue.Content = Game.Player.OwnedStars;
        }

        private void AutoColonizeCheckBox_Checked(object sender, RoutedEventArgs e) {
            Game.IsAutoColonizationEnabled = true;
        }

        private void AutoColonizeCheckBox_Unchecked(object sender, RoutedEventArgs e) {
            Game.IsAutoColonizationEnabled = false;
        }

        private void ShowTotalPopulationInReadableFormat_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if(sender != null && sender is Label valueLabel) {
                MessageBox.Show(HelperConvertFunctions.NumberToString(Game.Player.TotalPopulation), valueLabel.Name);
            }
        }

        private void NextHunderedTurnButton_Click(object sender, RoutedEventArgs e) {
            for (int i = 0; i < 100; i++) {
                Game.NextTurn();
            }
            RefreshGUI();
        }
    }
}
