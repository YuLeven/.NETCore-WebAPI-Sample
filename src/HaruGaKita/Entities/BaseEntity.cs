using System;
using System.ComponentModel.DataAnnotations;

namespace HaruGaKita.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}