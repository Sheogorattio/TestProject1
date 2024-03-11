using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestProject1.DAL.DTO;

namespace TestProject1.DAL.DAO
{
    internal class UserDao
    {
        public static bool DeleteUser(User user, bool hardMode = false)
        {
            ArgumentNullException.ThrowIfNull(user);
            if (user.Id == default)
            {
                throw new ArgumentException(
                    "Id field value must not be default",
                    "user.Id");
            }
            using var cmd = new SqlCommand(null, App.MsSqlConnection);
            if (hardMode)
            {
                cmd.CommandText = $"DELETE FROM Users WHERE Id=`{user.Id}`";
            }
            else
            {
                cmd.CommandText = $"UPDATE Users SET DeleteDt = CURRENT_TIMESTAMP, Name='', Birthdate=NULL WHERE Id='{user.Id}'";
            }

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                App.LogError(ex.Message);
                return false;
            }
        }


        public static bool UpdateUser(User user)
        {
            ArgumentNullException.ThrowIfNull(user);
            if (user.Id == default)
            {
                throw new ArgumentException(
                    "Id field value must not be default",
                    "user.Id");
            }
            using var cmd = new SqlCommand(
                $"UPDATE Users SET Name=@name, Login=@login, PasswordHash=@passHash, Birthdate=@birthdate" +
                $" WHERE Id = @id",
                App.MsSqlConnection);
            cmd.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar, 64)
            {
                Value = user.Name
            });
            cmd.Parameters.Add(new SqlParameter("@login", System.Data.SqlDbType.VarChar, 64)
            {
                Value = user.Login
            });
            cmd.Parameters.Add(new SqlParameter("@passHash", System.Data.SqlDbType.Char, 32)
            {
                Value = user.PasswordHash
            });
            cmd.Parameters.Add(new SqlParameter("@birthdate", System.Data.SqlDbType.DateTime)
            {
                Value = (object?)user.Birthdate ?? DBNull.Value
            });
            cmd.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.UniqueIdentifier)
            {
                Value = user.Id
            });

            try
            {
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                App.LogError(ex.Message);
                return false;
            }
        }


        public static List<User> GetAll(bool showDeleted = false)
        {
            using SqlCommand cmd = new SqlCommand(
                "SELECT * FROM Users" + (showDeleted ? "" : " WHERE DeleteDt IS NULL"),
                App.MsSqlConnection);
            try
            {
                using SqlDataReader reader = cmd.ExecuteReader();
                List<User> users = [];
                while (reader.Read())
                {
                    users.Add(new(reader));
                }
                return users;
            }
            catch (Exception ex)
            {
                return null!;
            }
        }


        public static Boolean AddUser(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            using var cmd = new SqlCommand(
                $"INSERT INTO Users(Id, Name, Login, PasswordHash, Birthdate)" +
                $"VALUES(NEWID(), @name, @login, @passHash, @birthdate)",
                App.MsSqlConnection);
            cmd.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar, 64)
            {
                Value = user.Name
            });
            cmd.Parameters.Add(new SqlParameter("@login", System.Data.SqlDbType.VarChar, 64)
            {
                Value = user.Login
            });
            cmd.Parameters.Add(new SqlParameter("@passHash", System.Data.SqlDbType.Char, 32)
            {
                Value = user.PasswordHash
            });
            cmd.Parameters.Add(new SqlParameter("@birthdate", System.Data.SqlDbType.DateTime)
            {
                Value = user.Birthdate
            });

            try
            {
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static User? GetUserByCredentials(string login, string password)
        {
            ArgumentNullException.ThrowIfNull(login);
            ArgumentNullException.ThrowIfNull(password);


            using var cmd = new SqlCommand(
                "SELECT * FROM Users u WHERE u.Login = @login",
                App.MsSqlConnection);

            cmd.Parameters.Add(new SqlParameter("@login", System.Data.SqlDbType.VarChar, 64) 
            { 
                Value = login
            });

            try
            {
                cmd.Prepare();
                var reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }


            using var cmd2 = new SqlCommand(
                "SELECT * FROM Users u WHERE u.Login = @login AND u.PasswordHash = @passHash",
                App.MsSqlConnection);

            cmd2.Parameters.Add(new SqlParameter("@login", System.Data.SqlDbType.VarChar, 64)
            {
                Value = login
            });
            cmd2.Parameters.Add(new SqlParameter("@passHash", System.Data.SqlDbType.Char, 32)
            {
                Value = password
            });

            try
            {
                cmd2.Prepare();
                var reader2 = cmd2.ExecuteReader();
                if (reader2.Read())
                {
                    return new User(reader2);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
