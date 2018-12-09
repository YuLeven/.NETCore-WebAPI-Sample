using HaruGaKita.Application.Accounts.Models;
using HaruGaKita.Domain.Entities;
using HaruGaKita.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HaruGaKita.Application.Accounts.Queries
{
    class MeQueryHandler : IRequestHandler<MeQuery, UserDto>
    {
        private readonly HaruGaKitaDbContext _context;

        public MeQueryHandler(HaruGaKitaDbContext context)
        {
            _context = context;
        }

        public Task<UserDto> Handle(MeQuery request, CancellationToken cancellationToken)
        {
            var guid = Guid.Parse(request.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);
            return _context.Set<User>()
                    .Select(UserDto.Projection)
                    .Where(u => u.Id == guid)
                    .FirstOrDefaultAsync();
        }
    }
}
