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

namespace WpfTest {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        List<Family> families = new List<Family>();
        public MainWindow() {
            InitializeComponent();
            FamilyTree();
        }

        private void FamilyTree() {
            PlayerSystemsTree.ItemsSource = families;
        }

        private void TestButton_Click(object sender, RoutedEventArgs e) {
            Family family3 = new Family() { Name = "The Moe's" };
            family3.Members.Add(new FamilyMember() { Name = "Mark Moe", Age = 31 });
            family3.Members.Add(new FamilyMember() { Name = "Norma Moe", Age = 28 });
            families.Add(family3);

            PlayerSystemsTree.Items.Refresh();
        }

        private void GetObjButton_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show((PlayerSystemsTree.SelectedItem is FamilyMember).ToString());
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
