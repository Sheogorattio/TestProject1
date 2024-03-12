using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace TestProject1
{
    /// <summary>
    /// Interaction logic for EfWindow.xaml
    /// </summary>
    public partial class EfWindow : Window
    {
        public EfWindow()
        {
            App.EfDataContext.Database.Migrate();
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayStatistics();
        }


        private void DisplayStatistics()
        {
            ManagersCountLabel.Content = App.EfDataContext.Managers.Count();
            DepartmentsCountLabel.Content = App.EfDataContext.Departments.Count();
            ProductsCountLabel.Content = App.EfDataContext.Products.Count();
            SalesCountLabel.Content = App.EfDataContext.Sales.Count();
        }


        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBox.Text = "";

            foreach (var dep in App.EfDataContext.Departments)
            {
                ResultTextBox.Text += $"{dep.Name} \n";
            }

            var query = App.EfDataContext
                .Departments
                .Where(d => d.InternationalName != null)
                .Take(10)
                .OrderBy(d => d.Name);

            foreach (var dep in query)
            {
                ResultTextBox.Text += $"int - {dep.InternationalName} \n";
            }

            var query2 = App.EfDataContext
                .Departments
                .Select(d => d.Name);

            ResultTextBox.Text += query2.Min() + "\n";

            var query3 = App.EfDataContext
                .Departments
                .OrderBy(d => d.Id)
                .AsEnumerable()
                .Select(d => new { Card = d.Id.ToString()[..5] + d.Name })
                .OrderBy(a => a.Card)
                .First();

            ResultTextBox.Text += query3.Card + "\n";
        }


        private void CheapButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBox.Text = "";

            var query = App.EfDataContext
                .Products
                .OrderBy(prod => prod.Price)
                .Select(prod => prod.Name + ":   " + prod.Price + "₴")
                .First();

            ResultTextBox.Text += query + "\n";

            var query2 = App.EfDataContext
                .Products
                .Average(prod => prod.Price);

            ResultTextBox.Text += "Середня ціна:   " + query2 + "₴\n";
        }


        private void AddSalesButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBox.Text = "";

            SalesCountLabel.Content = "Updating...";
            Task.Run(AddSales);
        }

        private async Task AddSales()
        {
            for (int i = 0; i < 10000; i++)
            {
                App.EfDataContext.Sales.Add(new EfContext.Sale()
                {
                    Id = Guid.NewGuid(),
                    ManagerId = App.EfDataContext.Managers.OrderBy(r => Guid.NewGuid()).First().Id,
                    ProductId = App.EfDataContext.Products.OrderBy(r => Guid.NewGuid()).First().Id,
                    Quantity = App.Random.Next(1, 10),
                    SaleDt = new DateTime(2023, 1, 1)   
                        .AddDays(App.Random.Next(365))
                        .AddHours(App.Random.Next(9, 20))
                        .AddMinutes(App.Random.Next(0, 59))
                        .AddSeconds(App.Random.Next(0, 59)),
                });
            }

            await App.EfDataContext.SaveChangesAsync();
            Dispatcher.Invoke(DisplayStatistics);
        }


        private void ProductSalesButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBox.Text = "";

            DateTime date = DateTime.Today.AddYears(-1);
            ResultTextBox.Text += $"{date.ToString("dd/MM/yyyy")}\n\n";

            var query = App.EfDataContext.Products
                .GroupJoin(
                    App.EfDataContext.Sales,
                    p => p.Id,
                    s => s.ProductId,
                    (product, sales) => new
                    {
                        Name = product.Name,
                        Pcs = sales.Where(s => s.SaleDt.Date == date).Sum(s => s.Quantity)
                    }
                );

            foreach ( var item in query )
            {
                ResultTextBox.Text += $"{item.Name}   {item.Pcs}\n";
            }
        }


        private void QueriesButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBox.Text = "";

            DateTime date = DateTime.Today.AddYears(-1);
            ResultTextBox.Text += $"{date.ToString("dd/MM/yyyy")}\n\n";

            var query = App.EfDataContext.Managers
                .GroupJoin(
                    App.EfDataContext.Sales,
                    m => m.Id,
                    s => s.ManagerId,
                    (manager, sales) => new
                    {
                        Surname = manager.Surname,
                        Name = manager.Name,
                        Secname = manager.Secname,

                        Pcs = sales.Where(s => s.SaleDt.Date == date).Sum(s => s.Quantity)
                    }
                );

            foreach (var manager in query)
            {
                ResultTextBox.Text += $"{manager.Surname} {manager.Name} {manager.Secname}   {manager.Pcs}\n";
            }
        }


        private void Nav1Button_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBox.Text = "";

            foreach(var str in
                App.EfDataContext
                    .Managers
                    .Include(manager => manager.MainDepartment)
                    .Select(manager => $"{manager.Surname}   {manager.MainDepartment.Name}")
                    .Take(10))
            {
                ResultTextBox.Text += $"{str}\n";
            }

            ResultTextBox.Text += "\n";
            foreach (var str in
                App.EfDataContext
                    .Departments
                    .Include(department => department.MainEmployees)
                    .Select(department => $"{department.Name}   {department.MainEmployees.Count}"))
            {
                ResultTextBox.Text += $"{str}\n";
            }
        }


        private void Nav2Button_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBox.Text = "";

            foreach (var (str, index) in
                App.EfDataContext
                    .Managers
                    .Include(manager => manager.SecondaryDepartment)
                    .ToList()
                    .Select((manager, index) => ($"{index + 1}. {manager.Surname }   {(manager.SecondaryDepartment == null ? new String("-") : manager.SecondaryDepartment.Name)}"))
                    .Select((str, index) => (str, index)))
            {
                ResultTextBox.Text += $"{str}\n";
            }
        }


        private void ChiefButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBox.Text = "";

            foreach (var manager in
                App.EfDataContext
                    .Managers
                    .Include(manager => manager.Chief))
            {
                ResultTextBox.Text += $"{manager.Surname} — {manager.Chief?.Surname ?? "no chief"}\n";
            }
        }


        private void SubordinatesButton_Click(object sender, RoutedEventArgs e)
        {
            ResultTextBox.Text = "";

            foreach (var manager in
                App.EfDataContext
                    .Managers
                    .Include(manager => manager.Subordinates))
            {
                ResultTextBox.Text += $"{manager.Surname} — {String.Join(", ", manager.Subordinates.Select(manager => manager.Surname))}\n";
            }
        }


        private void SalesProdsButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = new(2023, 02, 28);

            ResultTextBox.Text = "";

            foreach (Product product in
                App.EfDataContext
                    .Products
                    .Include(product => product.Sales).ToList())
            {
                int checksToday = product.Sales.Where(s =>s.SaleDt.Date == date).Count();
                int productsToday = 0;
                foreach (var q in product.Sales.Where(s => s.SaleDt.Date == date).Select(s => s.Quantity))
                {
                    productsToday+=q;
                }
                double Price = productsToday * product.Price;
                ResultTextBox.Text += $"{product.Name} — {checksToday} - {productsToday} - {Price}\n";
            }
        }
    }
}
