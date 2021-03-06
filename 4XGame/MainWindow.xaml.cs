﻿using System;
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

        [field: NonSerialized]
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
            Exit(null);
        }

        private void CurrentWindow_Closing(object sender, CancelEventArgs e) {
            Exit(e);
        }

        private void Exit(CancelEventArgs e) {
            var result = MessageBox.Show("Save before exit?", "Exit", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes) {
                if (ViewModel.SaveGameCmd.CanExecute(null)) {
                    this.ViewModel.SaveGameCmd.Execute(null);
                }
                KillApp();
            }
            else if(result == MessageBoxResult.No) {
                KillApp();
            }
            else if (result == MessageBoxResult.Cancel && e != null) {
                e.Cancel = true;
            }
        }

        private static void KillApp() {
            Process.GetCurrentProcess().Kill();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {

        }
    }
}
