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
                viewModel.SelectedStar = star;
            }
        }

        private void WritePlanetInfoToBox() {
            if (SystemPlanetsListBox.SelectedItem != null && SystemPlanetsListBox.SelectedItem is Planet planet) {
                viewModel.SelectedPlanet = planet;
            }
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
