using System;
using System.Threading.Tasks;
using HaruGaKita.Entities;

namespace HaruGaKita.Infrastructure.Interfaces
{
    public interface IUserRepository : IAsyncRepository<User>
    { }
}