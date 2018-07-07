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

namespace _4XGame {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
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
            SystemsGrid.ItemsSource = Game.Player.StarSystems;
        }

        private void RefreshGUI() {
            SystemsGrid.Items.Refresh();

            WriteStarInfoToBox();
            WritePlanetInfoToBox();

            ShowPlayerMoney.Content = $"{Game.Player.PlayerMoney:0.0000E0}";
            ShowCitizenHub.Content = $"{Game.Player.PlayerCitizenHub.CitizensInHub:0.0000E0}";
            ShowPlayerTotalPopulation.Content = $"{Game.Player.TotalPopulation:0.0000E0}";

            ShowPlayerHydrogen.Content = $"{Game.Player.PlayerResourses.Hydrogen:0.0000E0}";
            ShowPlayerMetals.Content = $"{Game.Player.PlayerResourses.CommonMetals:0.0000E0}";
            ShowPlayerRareMetals.Content = $"{Game.Player.PlayerResourses.RareEarthElements:0.0000E0}";

            RefreshColonizedPlanetsValueLabel();
            RefreshOwnedPlanetsValueLabel();
            RefreshOwnedStarsValueLabel();
            RefreshOwnedSystemsValueLabel();

            TurnLabelValue.Content = Game.GameTurn;
            DateLabelValue.Content = Game.GameDate;
        }

        #region Celestial Info Box
        private void PlayerStarsTree_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            WriteStarInfoToBox();
        }

        private void PlayerPlanetsTree_MouseLeftButtonUp(object sender, MouseButtonEventArgs e) {
            WritePlanetInfoToBox();
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
        #endregion

        #region Refresh Info Tab
        private void RefreshOwnedSystemsValueLabel() {
            OwnedSystemsValueLabel.Content = Game.Player.StarSystems.Count;
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
        #endregion

        #region Auto Colonization
        private void AutoColonizeCheckBox_Checked(object sender, RoutedEventArgs e) {
            Game.IsAutoColonizationEnabled = true;
        }

        private void AutoColonizeCheckBox_Unchecked(object sender, RoutedEventArgs e) {
            Game.IsAutoColonizationEnabled = false;
        }
        #endregion

        #region Next Turn
        private void NextTurnButton_Click(object sender, RoutedEventArgs e) {
            Game.NextTurn();
            RefreshGUI();
        }

        private void NextNTurnButton_Click(object sender, RoutedEventArgs e) {
            Stopwatch stopwatch = Stopwatch.StartNew();
            DisableUI();

            TurnsProgressBar.Value = 0;

            int turns = 0;
            if (!Int32.TryParse(TurnsNumberTextBox.Text, out turns)) {
                TurnsNumberTextBox.Text = String.Empty;
                return;
            }

            TurnsProgressBar.Maximum = turns;
            for (int i = 0; i < turns; i++) {
                Game.NextTurn();
                Dispatcher.Invoke(new Action(() => { TurnsProgressBar.Value++; }), DispatcherPriority.Background);
            }

            RefreshGUI();
            EnableUI();

            stopwatch.Stop();
            ElapsedTurnTimeValueLabel.Content = stopwatch.Elapsed;

            //Console.Beep();
        }

        private void DisableUI() {
            this.IsEnabled = false;
        }

        private void EnableUI() {
            this.IsEnabled = true;
        }
        #endregion

        private void ColonizePlanet_Click(object sender, RoutedEventArgs e) {
            if (SystemPlanetsListBox.SelectedItem != null && SystemPlanetsListBox.SelectedItem is Planet planet) {
                planet.Colonize(Game.Player);
                RefreshColonizedPlanetsValueLabel();
            }
        }

        private void ShowTotalPopulationInReadableFormat_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if(sender != null && sender is Label valueLabel) {
                MessageBox.Show(HelperConvertFunctions.NumberToString(Game.Player.TotalPopulation), valueLabel.Name);
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
