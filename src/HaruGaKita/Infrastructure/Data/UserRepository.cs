using HaruGaKita.Entities;
using HaruGaKita.Infrastructure.Interfaces;

namespace HaruGaKita.Infrastructure.Data
{
    public class UserRepository : EntityFrameworkRepository<User>, IUserRepository
    {
        public UserRepository(HaruGaKitaContext context) : base(context) { }
    }
}