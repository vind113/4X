using System;
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

namespace _4XGame {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            SetItemsSource();
        }

        private void SetItemsSource() {
            PlayerPlanetsTree.ItemsSource = Game.Player.StarSystems;
            PlayerStarsTree.ItemsSource = Game.Player.StarSystems;
        }

        private void NextTurnButton_Click(object sender, RoutedEventArgs e) {
            Game.NextTurn();
            
            //сворачивает вкладку, исправь
            PlayerStarsTree.Items.Refresh();
            PlayerPlanetsTree.Items.Refresh();

            WriteStarInfoToBox();
            WritePlanetInfoToBox();

            ShowPlayerMoney.Content = $"{Game.Player.PlayerMoney:0.0000E0}";
            ShowCitizenHub.Content = $"{Game.Player.PlayerCitizenHub.CitizensInHub:0.0000E0}";

            ShowPlayerHydrogen.Content = $"{Game.Player.PlayerResourses.Hydrogen:0.0000E0}";
            ShowPlayerMetals.Content = $"{Game.Player.PlayerResourses.CommonMetals:0.0000E0}";
            ShowPlayerRareMetals.Content = $"{Game.Player.PlayerResourses.RareEarthElements:0.0000E0}";

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
            }
        }

        private void ColonizePlanet_Click(object sender, RoutedEventArgs e) {
            if (PlayerPlanetsTree.SelectedItem != null && PlayerPlanetsTree.SelectedItem is Planet planet) {
                planet.Colonize();
            }
        }
    }
}
