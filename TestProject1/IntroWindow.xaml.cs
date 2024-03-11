using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Runtime.Intrinsics.Arm;
using Google.Protobuf.WellKnownTypes;

namespace TestProject1
{
    /// <summary>
    /// Interaction logic for IntroWindow.xaml
    /// </summary>
    public partial class IntroWindow : Window
    {
        private readonly String _msConnectionString;
        private readonly String _myConnectionString;

        private SqlConnection? sqlConnection;
        private MySqlConnection? mySqlConnection;


        public IntroWindow()
        {
            InitializeComponent();

            var config = JsonSerializer.Deserialize<JsonElement>(
                System.IO.File.ReadAllText("appsettings.json")
            );

            var connectionStrings = config.GetProperty("ConnectionStrings");

            _msConnectionString = connectionStrings.GetProperty("LocalMS").GetString()!;
            _myConnectionString = connectionStrings.GetProperty("LocalMy").GetString()!;
        }


        private void ConnectMsButton_Click(object sender, RoutedEventArgs e)
        {
            sqlConnection = new SqlConnection(_msConnectionString);

            try
            {
                sqlConnection.Open();
                MsConnectionStatusLabel.Content = "Connected";

                ConnectMsButton.IsEnabled = false;
                DisconnectMsButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MsConnectionStatusLabel.Content = ex.Message;
            }
        }

        private void DisconnectMsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sqlConnection != null)
            {
                sqlConnection = null;
            }
            MsConnectionStatusLabel.Content = "Disconnected";

            ConnectMsButton.IsEnabled = true;
            DisconnectMsButton.IsEnabled = false;
        }


        private void ConnectMyButton_Click(object sender, RoutedEventArgs e)
        {
            mySqlConnection = new MySqlConnection(_myConnectionString);

            try
            {
                mySqlConnection.Open();
                MyConnectionStatusLabel.Content = "Connected";

                ConnectMyButton.IsEnabled = false;
                DisconnectMyButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MyConnectionStatusLabel.Content = ex.Message;
            }
        }

        private void DisconnectMyButton_Click(object sender, RoutedEventArgs e)
        {
            if (mySqlConnection != null)
            {
                mySqlConnection = null;
            }
            MyConnectionStatusLabel.Content = "Disconnected";

            ConnectMyButton.IsEnabled = true;
            DisconnectMyButton.IsEnabled = false;
        }


        private void CreateMsButton_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand cmd = new();
            cmd.Connection = sqlConnection;
            cmd.CommandText = @"
                CREATE TABLE Users (
                    Id UNIQUEIDENTIFIER PRIMARY KEY,
                    Name NVARCHAR(64) NOT NULL,                
                    Login NVARCHAR(64) NOT NULL,
                    PasswordHash CHAR(32) NOT NULL
                )";

            try
            {
                cmd.ExecuteNonQuery();
                MsTableStatusLabel.Content = "Create OK";
            }
            catch (Exception ex)
            {
                MsTableStatusLabel.Content = ex.Message;
            }
        }

        private void DropMsButton_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand cmd = new();
            cmd.Connection = sqlConnection;
            cmd.CommandText = @"DROP TABLE Users";

            try
            {
                cmd.ExecuteNonQuery();
                MsTableStatusLabel.Content = "Drop OK";
            }
            catch (Exception ex)
            {
                MsTableStatusLabel.Content = ex.Message;
            }
        }

        private void AlterMsButton_Click(object sender, RoutedEventArgs e)
        {
            using SqlCommand cmd = new();
            cmd.Connection = sqlConnection;
            cmd.CommandText = @"
                ALTER TABLE Users
                ADD Birthdate DATE,
                DeleteDt DATETIME";

            try
            {
                cmd.ExecuteNonQuery();
                MsTableStatusLabel.Content = "Alter OK";
            }
            catch (Exception ex)
            {
                MsTableStatusLabel.Content = ex.Message;
            }
        }


        private void CreateMyButton_Click(object sender, RoutedEventArgs e)
        {
            using MySqlCommand cmd = new();
            cmd.Connection = mySqlConnection;
            cmd.CommandText = @"
                CREATE TABLE Users (
                    Id CHAR(36) PRIMARY KEY,
                    Name NVARCHAR(64) NOT NULL,     
                    Login NVARCHAR(64) NOT NULL,
                    PasswordHash CHAR(32) NOT NULL
                ) Engine = InnoDb, DEFAULT CHARSET = utf8mb4";

            try
            {
                cmd.ExecuteNonQuery();
                MyTableStatusLabel.Content = "Create OK";
            }
            catch (Exception ex)
            {
                MyTableStatusLabel.Content = ex.Message;
            }
        }

        private void DropMyButton_Click(object sender, RoutedEventArgs e)
        {
            using MySqlCommand cmd = new();
            cmd.Connection = mySqlConnection;
            cmd.CommandText = @"DROP TABLE Users";

            try
            {
                cmd.ExecuteNonQuery();
                MyTableStatusLabel.Content = "Drop OK";
            }
            catch (Exception ex)
            {
                MyTableStatusLabel.Content = ex.Message;
            }
        }

        private void AlterMyButton_Click(object sender, RoutedEventArgs e)
        {
            using MySqlCommand cmd = new();
            cmd.Connection = mySqlConnection;
            cmd.CommandText = @"
                ALTER TABLE Users
                ADD Birthdate DATE";

            try
            {
                cmd.ExecuteNonQuery();
                MyTableStatusLabel.Content = "Alter OK";
            }
            catch (Exception ex)
            {
                MyTableStatusLabel.Content = ex.Message;
            }
        }


        private String? GetInputError()
        {
            if (String.IsNullOrEmpty(UserNameTextBox.Text))
            {
                return "Enter your name";
            }
            if (String.IsNullOrEmpty(UserLoginTextBox.Text))
            {
                return "Enter your login";
            }
            if (String.IsNullOrEmpty(UserPasswordTextBox.Password))
            {
                return "Enter your password";
            }
            if (!UserBirthdateDatePicker.SelectedDate.HasValue)
            {
                return "Enter your date of birth";
            }
            return null;
        }


        private String md5(string input)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.MD5.HashData(
                    System.Text.Encoding.UTF8.GetBytes(input)));
        }


        private void InsertMsButton_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = GetInputError();
            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage);
                return;
            }

            using var cmd = new SqlCommand(
                $"INSERT INTO Users VALUES( NEWID(), @name, @login, '{md5(UserPasswordTextBox.Password)}', @birthdate, NULL)",
                sqlConnection
            );

            cmd.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar, 100)
            {
                Value = UserNameTextBox.Text
            });
            cmd.Parameters.Add(new SqlParameter("@login", System.Data.SqlDbType.VarChar, 100)
            {
                Value = UserLoginTextBox.Text
            });
            cmd.Parameters.Add(new SqlParameter("@birthdate", System.Data.SqlDbType.Date)
            {
                Value = UserBirthdateDatePicker.SelectedDate.Value.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture)
            });

            try
            {
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                InsertStatusLabel.Content = "Insert OK";
            }
            catch (Exception ex)
            {
                InsertStatusLabel.Content = ex.Message;
            }
        }

        private void InsertMyButton_Click(object sender, RoutedEventArgs e)
        {
            var errorMessage = GetInputError();
            if (errorMessage != null)
            {
                MessageBox.Show(errorMessage);
                return;
            }

            using var cmd = new MySqlCommand(
                $"INSERT INTO Users VALUES( UUID(), '{UserNameTextBox.Text}', '{UserLoginTextBox.Text}', '{md5(UserPasswordTextBox.Password)}', '{UserBirthdateDatePicker.SelectedDate.Value.ToString("yyyy-MM-dd")}')",
                mySqlConnection
            );

            try
            {
                cmd.ExecuteNonQuery();
                InsertStatusLabel.Content = "Insert OK";
            }
            catch (Exception ex)
            {
                InsertStatusLabel.Content = ex.Message;
            }
        }


        private void SelectMsButton_Click(object sender, RoutedEventArgs e)
        {
            if (sqlConnection == null ||
                sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                MessageBox.Show(
                    "Необхідно встановити підключення",
                    "Виконання зупинене",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            using SqlCommand cmd = new SqlCommand("SELECT * FROM Users", sqlConnection);
            try
            {
                using SqlDataReader reader = cmd.ExecuteReader();

                SelectMsTextBlock.Text = "";
                while (reader.Read())
                {
                    var id = reader.GetGuid("Id").ToString();
                    var name = reader.GetString("Name");
                    var login = reader.GetString("Login");
                    var hash = reader.GetString("PasswordHash");
                    var birthdate = Convert.ToDateTime(reader["Birthdate"]).ToString("dd.MM.yyyy");

                    SelectMsTextBlock.Text += $"{id[..5]}... {name} {login} {hash[..5]}... {birthdate}\n";
                }
            }
            catch (Exception ex)
            {
                SelectMsTextBlock.Text = ex.Message;
            }
        }

        private void SelectMyButton_Click(object sender, RoutedEventArgs e)
        {
            if (mySqlConnection == null ||
                mySqlConnection.State == System.Data.ConnectionState.Closed)
            {
                MessageBox.Show(
                    "Необхідно встановити підключення",
                    "Виконання зупинене",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            using MySqlCommand cmd = new MySqlCommand("SELECT * FROM Users", mySqlConnection);
            try
            {
                using MySqlDataReader reader = cmd.ExecuteReader();

                SelectMyTextBlock.Text = "";
                while (reader.Read())
                {
                    var id = reader.GetGuid("Id").ToString();
                    var name = reader.GetString("Name");
                    var login = reader.GetString("Login");
                    var hash = reader.GetString("PasswordHash");
                    var birthdate = Convert.ToDateTime(reader["Birthdate"]).ToString("dd.MM.yyyy");

                    SelectMyTextBlock.Text += $"{id[..5]}... {name} {login} {hash[..5]}... {birthdate}\n";
                }
            }
            catch (Exception ex)
            {
                SelectMyTextBlock.Text = ex.Message;
            }
        }
    }
}
