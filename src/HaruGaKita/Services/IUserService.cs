using System.Security.Claims;
using System.Threading.Tasks;
using HaruGaKita.Domain.Entities;

#pragma warning disable CS1591
namespace HaruGaKita.Services
{
    public interface IUserService
    {
        Task<User> AuthenticateUser(string email, string password);
        Task<string> SignCredentials(string email, string password);
        Task<User> GetCurrentUser(ClaimsPrincipal user);
    }
}