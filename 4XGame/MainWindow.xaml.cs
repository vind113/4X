using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Logic.Space_Objects;
using _4XGame.ViewModel;

namespace _4XGame {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private MainWindowViewModel viewModel;

        public MainWindowViewModel ViewModel {
            get => this.viewModel;
            set => this.viewModel = value;
        }

        public MainWindow() {
            ViewModel = new MainWindowViewModel();

            InitializeComponent();

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
            WriteStarInfoToBox();
            WritePlanetInfoToBox();
            
            RefreshColonizedPlanetsValueLabel();
            RefreshOwnedPlanetsValueLabel();
            RefreshOwnedStarsValueLabel();
            RefreshOwnedSystemsValueLabel();
        }

        private void RefreshOwnedSystemsValueLabel() {
            //OwnedSystemsValueLabel.Content = ViewModel.ThisGame.Player.StarSystems.Count;
        }

        private void RefreshColonizedPlanetsValueLabel() {
            ColonizedPlanetsValue.Content = ViewModel.ThisGame.Player.ColonizedPlanets;
        }

        private void RefreshOwnedPlanetsValueLabel() {
            OwnedPlanetsValue.Content = ViewModel.ThisGame.Player.OwnedPlanets;
        }

        private void RefreshOwnedStarsValueLabel() {
            OwnedStarsValue.Content = ViewModel.ThisGame.Player.OwnedStars;
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
            ViewModel.ThisGame.IsAutoColonizationEnabled = true;
        }

        private void AutoColonizeCheckBox_Unchecked(object sender, RoutedEventArgs e) {
            ViewModel.ThisGame.IsAutoColonizationEnabled = false;
        }
        #endregion

        #region Next Turn
        private void TurnsNumberTextBox_TextChanged(object sender, TextChangedEventArgs e) {
            uint turns = 0;
            if (UInt32.TryParse(TurnsNumberTextBox.Text, out turns)) {
                NextNTurnButton.Content = $"{turns} Turns";
            }
            else {
                NextNTurnButton.Content = $"? Turns";
            }
        }
        #endregion

        private void ColonizePlanet_Click(object sender, RoutedEventArgs e) {
            if (SystemPlanetsListBox.SelectedItem != null && SystemPlanetsListBox.SelectedItem is Planet planet) {
                planet.Colonize(ViewModel.ThisGame.Player);
                RefreshColonizedPlanetsValueLabel();
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
