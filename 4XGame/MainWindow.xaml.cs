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
    }
}
