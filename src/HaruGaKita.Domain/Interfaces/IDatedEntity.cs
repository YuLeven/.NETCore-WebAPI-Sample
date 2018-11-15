using System;

#pragma warning disable 1591
namespace HaruGaKita.Domain.Interfaces
{
    public interface IDatedEntity
    {
        DateTime Created { get; set; }
        DateTime Updated { get; set; }
    }
}