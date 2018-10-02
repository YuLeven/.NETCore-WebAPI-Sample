using HaruGaKita.Infrastructure.Interfaces;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaruGaKita.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        
        [JsonIgnore]
        public string EncryptedPassword { get; set; }
    }
}