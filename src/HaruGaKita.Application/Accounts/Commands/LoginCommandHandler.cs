using HaruGaKita.Application.Accounts.Models;
using HaruGaKita.Common;
using HaruGaKita.Persistence;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Claims;
using HaruGaKita.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MediatR;
using HaruGaKita.Application.Exceptions;

namespace HaruGaKita.Application.Accounts.Commands
{
    class LoginCommandHandler : IRequestHandler<LoginCommand, OAuthCredentials>
    {
        private readonly HaruGaKitaDbContext _context;
        private static readonly SigningCredentials _signingCredentials = new SigningCredentials(Configuration.ApplicationSecurityKey, SecurityAlgorithms.HmacSha256);

        public LoginCommandHandler(HaruGaKitaDbContext context)
        {
            _context = context;
        }

        private async Task<User> AuthenticateUser(string email, string password)
        {
            var user = await _context.Set<User>().Where(u => u.Email == email).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new UnauthenticatedException("Invalid username or password");
            }

            return user;
        }

        private Claim[] BuildUserClaims(User user)
        {

            return new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Uid.ToString())
            };
        }

        public async Task<OAuthCredentials> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await AuthenticateUser(request.Username, request.Password);
            var claims = BuildUserClaims(user);
            var handler = new JwtSecurityTokenHandler();

            var tokenPayload = new JwtSecurityToken(
                issuer: Configuration.AppAuthority,
                audience: Configuration.ApiAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: _signingCredentials
            );

            var token = handler.WriteToken(tokenPayload);

            return new OAuthCredentials
            {
                Token = token
            };
        }
    }
}
