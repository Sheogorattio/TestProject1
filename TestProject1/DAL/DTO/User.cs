﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1.DAL.DTO
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime? Birthdate { get; set; }


        public User() { }

        public User(DbDataReader reader)
        {
            ArgumentNullException.ThrowIfNull(reader);

            Id = reader.GetGuid("Id");
            Name = reader.GetString("Name");
            Login = reader.GetString("Login");
            PasswordHash = reader.GetString("PasswordHash");
            Birthdate = reader.IsDBNull("Birthdate") ? null : reader.GetDateTime("Birthdate");
        }
    }
}
