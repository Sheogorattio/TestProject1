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
    /// Interaction logic for EfSaleCrudWindow.xaml
    /// </summary>
    public partial class EfSaleCrudWindow : Window
    {
        public CrudActions Action { get; private set; }
        public SaleModel Model { get; set; }
        public EfSaleCrudWindow(SaleModel model)
        {
            InitializeComponent();
            this.DataContext = this;
            this.Model = model;
            Action = CrudActions.None;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Action = CrudActions.Delete;
            this.DialogResult = false;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Action = CrudActions.Update;
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
