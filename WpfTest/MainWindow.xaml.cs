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

namespace WpfTest {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        List<Family> families = new List<Family>();
        public MainWindow() {
            InitializeComponent();
            FamilyTree();

            this.Closing += MainWindow_Closing;
            this.Closed += MainWindow_Closed;

            this.MouseMove += MainWindow_MouseMove;
            this.KeyDown += MainWindow_KeyDown;
        }

        private void FamilyTree() {
            PlayerSystemsTree.ItemsSource = families;
            PersonDataGrid.ItemsSource = families;
        }

        private void TestButton_Click(object sender, RoutedEventArgs e) {
            Family family3 = new Family() { Name = "The Moe's" };
            family3.Members.Add(new FamilyMember() { Name = "Mark Moe", Age = 31 });
            family3.Members.Add(new FamilyMember() { Name = "Norma Moe", Age = 28 });
            for (int i = 0; i < 1000; i++) {
                families.Add(family3);
            }
            

            //PlayerSystemsTree.Items.Refresh();
            PersonDataGrid.Items.Refresh();
        }

        private void GetObjButton_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show((PersonDataGrid.SelectedItem is Family).ToString());
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e) {
            foreach (Window window in Application.Current.Windows) {
                window.WindowState = WindowState.Minimized;
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e) {
            string message = "Do you really want to close the program?";

            MessageBoxResult result =
                MessageBox.Show(message, "My app", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.No) {
                e.Cancel = true;
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e) {
            MessageBox.Show("See ya!");
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e) {
            this.Close();
        }

        protected void MainWindow_MouseMove(object sender, MouseEventArgs e) {
            //  Установить  в  заголовке  окна  текущие  координаты  (x,  у)  мыши.
            this.Title = e.GetPosition(this).ToString();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e) {
            //  Отобразить  на  кнопке  нажатую  клавишу.
            ButtonInfo.Content = e.Key.ToString();
        }
    }

    public class Family {
        public Family() {
            this.Members = new ObservableCollection<FamilyMember>();
        }

        public string Name { get; set; }

        public ObservableCollection<FamilyMember> Members { get; set; }
    }

    public class FamilyMember {
        public string Name { get; set; }

        public int Age { get; set; }

        public override string ToString() {
            return $"{Name}:{Age}";
        }
    }
}
