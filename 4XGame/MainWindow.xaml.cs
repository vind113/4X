using System;
using System.Windows;
using System.Windows.Controls;
using _4XGame.ViewModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace _4XGame {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged {
        private MainWindowViewModel viewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel ViewModel {
            get => this.viewModel;
            set {
                this.viewModel = value;
                OnPropertyChanged();
            }
        }

        public MainWindow() {
            ViewModel = new MainWindowViewModel();
            InitializeComponent();
            this.Title = "4X Game";
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
