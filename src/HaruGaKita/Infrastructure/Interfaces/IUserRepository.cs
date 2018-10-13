using System.Threading.Tasks;
using HaruGaKita.Entities;

#pragma warning disable 1591
namespace HaruGaKita.Infrastructure.Interfaces
{
    public interface IUserRepository : IAsyncRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
