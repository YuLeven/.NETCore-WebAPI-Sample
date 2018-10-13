using System.Threading.Tasks;
using System.Linq;
using HaruGaKita.Entities;
using HaruGaKita.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

#pragma warning disable 1591
namespace HaruGaKita.Infrastructure.Data
{
    public class UserRepository : EntityFrameworkRepository<User>, IUserRepository
    {
        public UserRepository(HaruGaKitaContext context) : base(context) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Set<User>()
              .Where(u => u.Email == email)
              .FirstOrDefaultAsync();
        }
    }
}
