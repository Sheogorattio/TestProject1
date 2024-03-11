using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using TestProject1.Models;

namespace TestProject1.EfCrudViews
{
    /// <summary>
    /// Interaction logic for EfManagerCrudWindow.xaml
    /// </summary>
    public partial class EfManagerCrudWindow : Window
    {
        public ManagerModel Model { get; init; }
        public CrudActions Action { get; private set; }


        public EfManagerCrudWindow(ManagerModel model)
        {
            InitializeComponent();
            this.Model = model;
            this.DataContext = this;
            Action = CrudActions.None;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainDepComboBox.SelectedItem =
                this.Model.Departments.First(depName =>
                depName.Id == Model.MainDep.Id);
            if(Model.SecDep != null)
            {
                SecDepComboBox.SelectedItem =
                this.Model.Departments.First(depName =>
                          depName.Id == Model.SecDep.Id);
            }

            if(Model.Chief != null)
            {
                ChiefComboBox.SelectedItem = Model.Chiefs.First(m => m.Id == Model.Chief.Id); 
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Action = CrudActions.Delete;
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Action = CrudActions.Update;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Action = CrudActions.None;
            this.DialogResult = false;
        }

        private void ClearSecDep_Click(object sender, RoutedEventArgs e)
        {
            SecDepComboBox.SelectedItem = null;
        }

        private void ClearChief_Click(object sender, RoutedEventArgs e)
        {
            ChiefComboBox.SelectedIndex = -1;
        }

        private void ChiefComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.Chief = (IdName)ChiefComboBox.SelectedItem as IdName;
        }

        private void SecDepComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.SecDep = (IdName)SecDepComboBox.SelectedItem as IdName;
        }

        private void MainDepComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.MainDep = (MainDepComboBox.SelectedItem as IdName)!;
        }
    }
}
