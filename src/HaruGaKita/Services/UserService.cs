using System;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using HaruGaKita.Domain.Entities;
using HaruGaKita.Persistence;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HaruGaKita.Domain.Exceptions;

#pragma warning disable CS1591
namespace HaruGaKita.Services
{
    public class UserService : IUserService
    {
        private readonly HaruGaKitaDbContext _dbContext;
        private static readonly SigningCredentials _signingCredentials = new SigningCredentials(Common.Configuration.ApplicationSecurityKey, SecurityAlgorithms.HmacSha256);

        public UserService(HaruGaKitaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> AuthenticateUser(string email, string password)
        {
            var user = await _dbContext.Set<User>().Where(u => u.Email == email).FirstOrDefaultAsync();

            if (user == null)
            {
                throw new UnauthenticatedException("Invalid username or password");
            }

            return user;
        }

        public async Task<string> SignCredentials(string email, string password)
        {
            var user = await AuthenticateUser(email, password);
            var claims = BuildUserClaims(user);
            var handler = new JwtSecurityTokenHandler();

            var token = new JwtSecurityToken(
                issuer: Common.Configuration.AppAuthority,
                audience: Common.Configuration.ApiAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: _signingCredentials
            );

            return handler.WriteToken(token);
        }

        public async Task<User> GetCurrentUser(ClaimsPrincipal user)
        {
            var guid = Guid.Parse(user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value);
            return await _dbContext.Set<User>().Where(u => u.Uid == guid).FirstOrDefaultAsync();
        }

        private Claim[] BuildUserClaims(User user)
        {
            return new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Uid.ToString())
            };
        }
    }
}
