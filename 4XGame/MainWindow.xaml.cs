using System;
using System.Windows;
using System.Windows.Controls;
using _4XGame.ViewModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;

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
        
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e) {
            Process.GetCurrentProcess().Kill();
        }
    }
}
