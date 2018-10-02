using System;

namespace HaruGaKita.Infrastructure.Interfaces
{
    public interface IDatedEntity
    {
        DateTime Created { get; set; }
        DateTime Updated { get; set; }
    }
}