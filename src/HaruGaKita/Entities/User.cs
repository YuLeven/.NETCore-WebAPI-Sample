using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HaruGaKita.Infrastructure.Interfaces;
using Newtonsoft.Json;

#pragma warning disable 1591
namespace HaruGaKita.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        public string Email { get; set; }

        private string _encryptedPassword;

        [JsonIgnore]
        public string EncryptedPassword
        {
            get => _encryptedPassword;

            set
            {
                _encryptedPassword = BCrypt.Net.BCrypt.HashPassword(value);
            }
        }

        public User() { }

        public User(string username, string email, string password)
        {
            Username = username;
            Email = email;
            EncryptedPassword = password;
        }
    }
}
