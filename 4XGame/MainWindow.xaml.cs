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

            ShowPlayerMoney.Content = $"{Game.Player.PlayerMoney:0.0000E0}";
            ShowCitizenHub.Content = $"{Game.Player.PlayerCitizenHub.CitizensInHub:0.0000E0}";

            ShowPlayerHydrogen.Content = $"{Game.Player.PlayerResourses.Hydrogen:0.0000E0}";
            ShowPlayerMetals.Content = $"{Game.Player.PlayerResourses.CommonMetals:0.0000E0}";
            ShowPlayerRareMetals.Content = $"{Game.Player.PlayerResourses.RareEarthElements:0.0000E0}";

            ShowTurnLabel.Content = Game.GameTurn;
            ShowDateLabel.Content = Game.GameDate;
        }

        private void Colonize_Click(object sender, RoutedEventArgs e) {
            /*foreach (var system in Game.Player.StarSystems) {
                foreach (var star in system.SystemStars) {
                    MessageBox.Show(star.ToString());
                }
                foreach (var planet in system.SystemPlanets) {
                    MessageBox.Show(planet.ToString());
                }
            }*/
            if (PlayerPlanetsTree.SelectedItem != null && PlayerPlanetsTree.SelectedItem is Planet p) {
                p.Colonize();
            }
        }

        private void PlayerStarsTree_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (PlayerStarsTree.SelectedItem == null) {
#if DEBUG
                MessageBox.Show("No object");
#endif
                return;
            }
            MessageBox.Show(PlayerStarsTree.SelectedItem.ToString());
        }

        private void PlayerPlanetsTree_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (PlayerPlanetsTree.SelectedItem == null) {
#if DEBUG
                MessageBox.Show("No object");
#endif
                return;
            }
            MessageBox.Show(PlayerPlanetsTree.SelectedItem.ToString());
        }
    }
}
