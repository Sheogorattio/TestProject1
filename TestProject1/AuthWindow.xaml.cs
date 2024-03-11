using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using TestProject1.DAL.DAO;

namespace TestProject1
{
    /// <summary>
    /// Interaction logic for AuthWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }


        private String? GetLogInputError()
        {
            if (String.IsNullOrEmpty(LogLogin.Text))
            {
                return "Fill Login box";
            }
            if (String.IsNullOrEmpty(LogPassword.Password))
            {
                return "Fill Password box";
            }
            return null;
        }

        private String? GetRegInputError()
        {
            if (String.IsNullOrEmpty(RegName.Text))
            {
                return "Fill Name box";
            }
            if (String.IsNullOrEmpty(RegLogin.Text))
            {
                return "Fill Login box";
            }
            if (String.IsNullOrEmpty(RegPassword.Password))
            {
                return "Fill Password box";
            }
            if (RegPassword.Password != RegRepeat.Password)
            {
                return "Password mismatch";
            }
            return null;
        }


        


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = GetLogInputError();
            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage,
                    "Виконання зупинене",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            try
            {
                var loginUser = UserDao.GetUserByCredentials(LogLogin.Text, App.md5(LogPassword.Password));

                if (loginUser != null)
                {
                    MessageBox.Show("Login OK");
                }
                else
                {
                    MessageBox.Show("Login fails");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void RegButton_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = GetRegInputError();
            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage,
                    "Виконання зупинене",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            try
            {
                if (UserDao.AddUser(new()
                {
                    Name = RegName.Text,
                    Login = RegLogin.Text,
                    PasswordHash = App.md5(RegPassword.Password),
                    Birthdate = DateTime.Now,
                }))
                {
                    MessageBox.Show("Insert OK");
                }
                else
                {
                    MessageBox.Show("Insert fails");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
