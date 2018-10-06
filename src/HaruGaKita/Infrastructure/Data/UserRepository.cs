using System;
using System.Threading.Tasks;
using System.Linq;
using HaruGaKita.Entities;
using HaruGaKita.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HaruGaKita.Infrastructure.Data
{
    public class UserRepository : EntityFrameworkRepository<User>, IUserRepository
    {
        public UserRepository(HaruGaKitaContext context) : base(context) { }
    }
}