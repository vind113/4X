using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Threading;

using Logic.GameClasses;
using Logic.PlayerClasses;
using Logic.Space_Objects;
using Logic.SupportClasses;
using System.Diagnostics;
using System.Threading;

namespace _4XGame {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private Game thisGame = new Game();
        private UIViewModel viewModel = new UIViewModel();

        public MainWindow() {
            InitializeComponent();
            SetItemsSource();

            this.Title = "4X Game";

            InitializeSystemsGrid();

            RefreshGUI();
        }

        private void InitializeSystemsGrid() {
            AddColumn("Name", "Name");
            AddColumn("StarsCount", "Stars");
            AddColumn("PlanetsCount", "Planets");
            AddColumn("HabitablePlanets", "Habitable");
            AddColumn("ColonizedCount", "Colonized");
            AddColumn("SystemPopulation", "Population");
        }

        private void AddColumn(string path, string columnName) {
            DataGridTextColumn column = new DataGridTextColumn();
            Binding binding = new Binding(path);
            column.Binding = binding;
            column.Header = columnName;
            SystemsGrid.Columns.Add(column);
        }

        private void SetItemsSource() {
            SystemsGrid.ItemsSource = thisGame.Player.StarSystems;
        }

        #region Celestial Info Box
        private void PlayerStarsTree_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            WriteStarInfoToBox();
        }

        private void PlayerPlanetsTree_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            WritePlanetInfoToBox();
        }
        #endregion

        #region Refresh Info Panels
        private void RefreshGUI() {
            SystemsGrid.Items.Refresh();

            WriteStarInfoToBox();
            WritePlanetInfoToBox();

            RefreshTopPanel();

            RefreshColonizedPlanetsValueLabel();
            RefreshOwnedPlanetsValueLabel();
            RefreshOwnedStarsValueLabel();
            RefreshOwnedSystemsValueLabel();

            RefreshDateTurnPanel();
        }

        private void RefreshTopPanel() {
            ShowPlayerMoney.Content = $"{thisGame.Player.PlayerMoney:0.0000E0}";
            ShowCitizenHub.Content = $"{thisGame.Player.PlayerCitizenHub.CitizensInHub:0.0000E0}";
            ShowPlayerTotalPopulation.Content = $"{thisGame.Player.TotalPopulation:0.0000E0}";

            ShowPlayerHydrogen.Content = $"{thisGame.Player.PlayerResourses.Hydrogen:0.0000E0}";
            ShowPlayerMetals.Content = $"{thisGame.Player.PlayerResourses.CommonMetals:0.0000E0}";
            ShowPlayerRareMetals.Content = $"{thisGame.Player.PlayerResourses.RareEarthElements:0.0000E0}";
        }

        private void RefreshOwnedSystemsValueLabel() {
            OwnedSystemsValueLabel.Content = thisGame.Player.StarSystems.Count;
        }

        private void RefreshColonizedPlanetsValueLabel() {
            ColonizedPlanetsValue.Content = thisGame.Player.ColonizedPlanets;
        }

        private void RefreshOwnedPlanetsValueLabel() {
            OwnedPlanetsValue.Content = thisGame.Player.OwnedPlanets;
        }

        private void RefreshOwnedStarsValueLabel() {
            OwnedStarsValue.Content = thisGame.Player.OwnedStars;
        }

        private void RefreshDateTurnPanel() {
            TurnLabelValue.Content = thisGame.GameTurn;
            DateLabelValue.Content = thisGame.GameDate;
        }

        private void WriteStarInfoToBox() {
            if (SystemStarsListBox.SelectedItem != null && SystemStarsListBox.SelectedItem is Star star) {
                StarNameValue.Content = star.Name;
                StarRadiusValue.Content = $"{star.Radius} km";
                StarAreaValue.Content = $"{star.Area:E4} km^2";
                StarTypeValue.Content = star.LumClass;
            }
        }

        private void WritePlanetInfoToBox() {
            if (SystemPlanetsListBox.SelectedItem != null && SystemPlanetsListBox.SelectedItem is Planet planet) {
                PlanetNameValue.Content = planet.Name;
                PlanetRadiusValue.Content = $"{planet.Radius} km";
                PlanetAreaValue.Content = $"{planet.Area:E4} km^2";
                PlanetPopulationValue.Content = $"{planet.Population:E5}";
                PlanetTypeValue.Content = planet.Type.Name;

                PlanetHydrogenValueLabel.Content = $"{planet.BodyResourse.Hydrogen:E4} t";
                PlanetCommonMetalsValueLabel.Content = $"{planet.BodyResourse.CommonMetals:E4} t";
                PlanetRareMetalsValueLabel.Content = $"{planet.BodyResourse.RareEarthElements:E4} t";
            }
        }

        private void DisableUI() {
            SetTurnCriticalElements(false);
        }

        private void EnableUI() {
            SetTurnCriticalElements(true);
        }

        private void SetTurnCriticalElements(bool state) {
            NextNTurnButton.IsEnabled = state;
            NextTurnButton.IsEnabled = state;
            ColonizePlanetButton.IsEnabled = state;
        }
        #endregion

        #region Auto Colonization
        private void AutoColonizeCheckBox_Checked(object sender, RoutedEventArgs e) {
            thisGame.IsAutoColonizationEnabled = true;
        }

        private void AutoColonizeCheckBox_Unchecked(object sender, RoutedEventArgs e) {
            thisGame.IsAutoColonizationEnabled = false;
        }
        #endregion

        #region Next Turn
        private void NextTurnButton_Click(object sender, RoutedEventArgs e) {
            thisGame.NextTurn();
            RefreshGUI();
        }

        private void TurnsNumberTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            uint turns = 0;
            if (UInt32.TryParse(TurnsNumberTextBox.Text, out turns)) {
                NextNTurnButton.Content = $"{turns} Turns";
            }
            else {
                NextNTurnButton.Content = $"? Turns";
            }
        }

        private void NextNTurnButton_Click(object sender, RoutedEventArgs e) {
            Stopwatch stopwatch = Stopwatch.StartNew();

            TurnsProgressBar.Value = 0;

            int turns = 0;
            if (!Int32.TryParse(TurnsNumberTextBox.Text, out turns)) {
                TurnsNumberTextBox.Text = String.Empty;
                return;
            }
            
            DisableUI();

            TurnsProgressBar.Maximum = turns;
            for (int i = 0; i < turns; i++) {
                thisGame.NextTurn();
                Dispatcher.Invoke(new Action(() => { TurnsProgressBar.Value++; }), DispatcherPriority.Background);
            }

            RefreshGUI();
            EnableUI();

            stopwatch.Stop();
            ElapsedTurnTimeValueLabel.Content = stopwatch.Elapsed;

            //Console.Beep();
        }
        #endregion

        private void ColonizePlanet_Click(object sender, RoutedEventArgs e) {
            if (SystemPlanetsListBox.SelectedItem != null && SystemPlanetsListBox.SelectedItem is Planet planet) {
                planet.Colonize(thisGame.Player);
                RefreshColonizedPlanetsValueLabel();
            }
        }

        private void ShowTotalPopulationInReadableFormat_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if(sender != null && sender is Label valueLabel) {
                MessageBox.Show(HelperConvertFunctions.NumberToString(thisGame.Player.TotalPopulation), "Total population:");
            }
        }

        private void SystemsGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            if(SystemsGrid.SelectedItem != null && SystemsGrid.SelectedItem is StarSystem system) {
                SystemPlanetsListBox.ItemsSource = system.SystemPlanets;
                SystemStarsListBox.ItemsSource = system.SystemStars;
            }
        }
    }
}
