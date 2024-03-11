using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using TestProject1.EfContext;
using TestProject1.EfCrudViews;
using TestProject1.Models;

namespace TestProject1
{
    /// <summary>
    /// Interaction logic for EfCrudWindow.xaml
    /// </summary>
    public partial class EfCrudWindow : Window
    {
        private ICollectionView departmentsView;
        private ICollectionView managersView;
        private readonly Predicate<Object> departmentsFilter = obj => (obj as Department)?.DeleteDt == null;
        private Task? dbTask;
        public EfCrudWindow()
        {
            InitializeComponent();
        }


        private void LoadDepartmentsData()
        {
            DepartmentsListView.ItemsSource = null;

            App.EfDataContext.Departments.Load();
            DepartmentsListView.ItemsSource =
                App.EfDataContext
                .Departments
                .Local
                .ToObservableCollection();

            departmentsView = CollectionViewSource.GetDefaultView(DepartmentsListView.ItemsSource);
            departmentsView.Filter = departmentsFilter;
        }

        private void LoadManagersData()
        {
            ManagersListView.ItemsSource = null;

            App.EfDataContext.Managers.Load();
            ManagersListView.ItemsSource =
                App.EfDataContext
                .Managers
                .Local
                .ToObservableCollection();
        }

        private void LoadProductData()
        {
            ProductsListView.ItemsSource = null;

            App.EfDataContext.Products.Load();
            ProductsListView.ItemsSource =
                App.EfDataContext
                .Products
                .Local
                .ToObservableCollection();
        }

        private void LoadSalesData()
        {
            SalesListView.ItemsSource = null;

            App.EfDataContext.Sales.Load();
            SalesListView.ItemsSource =
                App.EfDataContext
                .Sales
                .Local
                .ToObservableCollection();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDepartmentsData();
            LoadManagersData();
            LoadProductData();
            LoadSalesData();
        }


        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item && 
                item.Content is Department department)
            {
                EfDepartmentCrudWindow dialog =
                        new(DepartmentModel.FromEntity(department));
                dialog.ShowDialog();

                if (dialog.Action == CrudActions.Update)
                {
                    department.Name = dialog.Model.Name;
                    department.InternationalName = dialog.Model.InternationalName;
                    App.EfDataContext.SaveChanges();

                    DepartmentsListView.Items.Refresh();
                }
                if (dialog.Action == CrudActions.Delete)
                {
                    department.DeleteDt = DateTime.Now;
                    App.EfDataContext.SaveChanges();

                    DepartmentsListView.Items.Refresh();
                }
            }
        }


        private void ListViewItem_MouseDoubleClick_2(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item && item.Content is Product product)
            {
                EfCrudProductWindow dialog =
                    new(ProductModel.FromEntity(product));
                dialog.ShowDialog();

                if(dialog.Action == CrudActions.Update)
                {
                    product.Price = dialog.Model.Price;
                    product.Name = dialog.Model.Name ?? null;
                    App.EfDataContext.SaveChanges();

                    ProductsListView.Items.Refresh();
                }
                if(dialog.Action == CrudActions.Delete)
                {
                    product.DeleteDt = DateTime.Now;
                    App.EfDataContext.SaveChanges();

                    ProductsListView.Items.Refresh();
                }
            }
        }


        private void AddDepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            Department department = new()
            {
                Id = Guid.NewGuid(),

            };

            EfDepartmentCrudWindow dialog =
                        new(DepartmentModel.FromEntity(department));
            dialog.ShowDialog();

            if (dialog.Action == CrudActions.Update)
            {
                department.Name = dialog.Model.Name;
                department.InternationalName = dialog.Model.InternationalName;
                App.EfDataContext.Add(department);
                App.EfDataContext.SaveChanges();

                DepartmentsListView.Items.Refresh();
            }
        }


        private void AllDepartmentsButton_Click(object sender, RoutedEventArgs e)
        {
            if (departmentsView.Filter == null)
            {
                departmentsView.Filter = departmentsFilter;
                AllDepartmentsButton.Content = "All";
            }
            else
            {
                departmentsView.Filter = null;
                AllDepartmentsButton.Content = "Hide";
            }
        }

        private void AddManagerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AllManagersButton_Click(object sender, RoutedEventArgs e)
        {

        }


        private void ListViewItem_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item && item.Content is Manager manager)
            {
                EfManagerCrudWindow dialog = new(
                        new ManagerModel(manager)
                        {
                            Departments = App.EfDataContext
                                             .Departments
                                             .OrderBy(x => x.Name)
                                             .Select(department =>
                                                new IdName { Id=department.Id,
                                                             Name = department.Name})
                                             .ToList(),

                            Chiefs =App.EfDataContext
                            .Managers
                            .Select(m=> new IdName
                            {
                                Id = m.Id,
                                Name = $"{m.Surname} {m.Name[0]}. {m.Surname[0]}",
                            }).ToList(),
                           
                        }); ;
                dialog.ShowDialog();
                

                if (dialog.Action == CrudActions.Update)
                {
                    manager.Surname = dialog.Model.Surname;
                    manager.Name = dialog.Model.Name;
                    manager.Secname = dialog.Model.Secname;
                    manager.IdMainDep = dialog.Model.MainDep.Id;
                    manager.IdSecDep = dialog.Model.SecDep?.Id;
                    manager.IdChief = dialog.Model.Chief?.Id;
                    dbTask = App.EfDataContext.SaveChangesAsync()
                        .ContinueWith(x => Dispatcher.Invoke(LoadManagersData));
                    
                }
                else if (dialog.Action == CrudActions.Delete)
                {
                    manager.DeleteDt = DateTime.Now;
                    App.EfDataContext.SaveChanges();

                    ManagersListView.Items.Refresh();
                }

                if(dialog.Action == CrudActions.None)
                {
                    dbTask = App.EfDataContext
                        .SaveChangesAsync()
                        .ContinueWith(_ => Dispatcher.Invoke(LoadManagersData));
                }
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
                dbTask?.Wait();  
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AllProductButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddSaleButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AllSaleButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListViewItem_MouseDoubleClick_3(object sender, MouseButtonEventArgs e)
        {
            if(sender is ListViewItem item && item.Content is Sale sale)
            {
                EfSaleCrudWindow dialog = new(SaleModel.Fromentity(sale));
                dialog.ShowDialog();

                if(dialog.Action == CrudActions.Update)
                {
                    sale.Quantity = dialog.Model.Quantity;
                    //App.EfDataContext.SaveChanges();

                    SalesListView.Items.Refresh();
                    dbTask = App.EfDataContext.SaveChangesAsync()
                       .ContinueWith(x => Dispatcher.Invoke(LoadManagersData));
                }
                if(dialog.Action == CrudActions.Delete)
                {
                    sale.DeleteDt = DateTime.Now;
                    App.EfDataContext.SaveChanges();

                    SalesListView.Items.Refresh();
                }
                if(dialog.Action == CrudActions.None)
                {
                    dbTask = App.EfDataContext.SaveChangesAsync()
                       .ContinueWith(x => Dispatcher.Invoke(LoadManagersData));
                }
            }
        }
    }
}