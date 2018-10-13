using System;
using HaruGaKita.Entities;
using HaruGaKita.Exceptions;
using HaruGaKita.Infrastructure.Interfaces;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

#pragma warning disable CS1591
namespace HaruGaKita.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private static readonly SigningCredentials _signingCredentials = new SigningCredentials(HaruGaKita.Configuration.ApplicationSecurityKey, SecurityAlgorithms.HmacSha256);

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AuthenticateUser(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null || !IsPasswordValid(user, password))
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
                issuer: HaruGaKita.Configuration.AppAuthority,
                audience: HaruGaKita.Configuration.ApiAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: _signingCredentials
            );

            return handler.WriteToken(token);
        }

        public async Task<User> GetCurrentUser(ClaimsPrincipal user)
        {
            return await _userRepository.GetByGuidAsync(Guid.Parse(user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value));
        }

        private Claim[] BuildUserClaims(User user)
        {
            return new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Uid.ToString())
            };
        }

        private bool IsPasswordValid(User user, string password) => BCrypt.Net.BCrypt.Verify(password, user.EncryptedPassword);
    }
}
