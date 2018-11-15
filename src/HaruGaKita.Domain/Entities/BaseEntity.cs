using System;
using HaruGaKita.Domain.Interfaces;

#pragma warning disable 1591
namespace HaruGaKita.Domain.Entities
{
    public class BaseEntity : IDatedEntity
    {
        public long Id { get; set; }

        public Guid Uid { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
}