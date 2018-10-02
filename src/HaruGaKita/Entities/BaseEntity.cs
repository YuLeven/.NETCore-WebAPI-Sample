using System;
using System.ComponentModel.DataAnnotations;
using HaruGaKita.Infrastructure.Interfaces;
using Newtonsoft.Json;

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