using System;

#pragma warning disable 1591
namespace HaruGaKita.Infrastructure.Interfaces
{
    public interface IDatedEntity
    {
        DateTime Created { get; set; }
        DateTime Updated { get; set; }
    }
}