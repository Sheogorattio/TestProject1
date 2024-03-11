using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows;

namespace TestProject1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static EfContext.EfContext EfDataContext { get; } = new();
        public static Random Random { get; } = new();

        public static void LogError(
            string message,
            [CallerMemberNameAttribute] string callerName = "undefined")
        { 
            System.IO.File.AppendAllText(
                "logs.txt",
                $"{DateTime.Now} [{callerName}] {message}\n"
            );
        }


        private static SqlConnection? _msConnection;
        public static SqlConnection MsSqlConnection 
        { 
            get
            {
                if (_msConnection == null)
                {
                    _msConnection = new(
                        JsonSerializer.Deserialize<JsonElement>(
                            System.IO.File.ReadAllText("appsettings.json")
                        )
                        .GetProperty("ConnectionStrings")
                        .GetProperty("LocalMS")
                        .GetString()!
                    );
                    _msConnection.Open();
                }
                return _msConnection;
            }
        }


        public static String md5(string input)
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.MD5.HashData(
                    System.Text.Encoding.UTF8.GetBytes(input)));
        }
    }

}
