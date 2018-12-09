using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HaruGaKita.Application.Accounts.Models;
using HaruGaKita.Domain.Entities;
using HaruGaKita.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HaruGaKita.Application.Accounts.Commands
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, UserDto>
    {
        private readonly HaruGaKitaDbContext _context;

        public CreateAccountCommandHandler(HaruGaKitaDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Email = request.Email,
                EncryptedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Uid = Guid.NewGuid()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return await _context.Set<User>()
                    .Select(UserDto.Projection)
                    .Where(u => u.Id == user.Uid)
                    .FirstOrDefaultAsync();
        }
    }
}