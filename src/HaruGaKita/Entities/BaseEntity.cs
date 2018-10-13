using System;
using System.ComponentModel.DataAnnotations;
using HaruGaKita.Infrastructure.Interfaces;
using Newtonsoft.Json;

#pragma warning disable 1591
namespace HaruGaKita.Entities
{
    public class BaseEntity : IDatedEntity
    {
        [Key]
        [JsonIgnore]
        public long Id { get; set; }

        [JsonProperty("id")]
        public Guid Uid { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
}