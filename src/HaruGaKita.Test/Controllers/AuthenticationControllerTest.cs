using Xunit;
using HaruGaKita.WebAPI.Controllers;
using Moq;
using HaruGaKita.Test.Support;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using HaruGaKita.Models;
using HaruGaKita.Domain.Entities;

namespace HaruGaKita.Test.Controllers
{
    public class AuthenticationControllerTest : IntegrationTest
    {
        private AuthenticationController _controller;
        private readonly User _validUser;

        public AuthenticationControllerTest() : base()
        {
            _validUser = Factories.UserFactory.Generate();
            _controller = new AuthenticationController(UserService);
        }

        [Fact]
        public async void Login_Returns_A_Token_When_Valid_Credentials_Are_Given()
        {
            _validUser.EncryptedPassword = "secret";
            await DbContext.AddAsync(_validUser);

            var body = new LoginRequest
            {
                Username = _validUser.Email,
                Password = "secret"
            };

            var response = await _controller.Login(body);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadJwtToken(response.Value.Token);
            var tokenUserId = jsonToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;

            Assert.NotNull(response.Value.Token);
            Assert.Equal(_validUser.Uid.ToString(), tokenUserId);
        }

        [Fact]
        public async void Login_Returns_401_When_Invalid_Credentials_Are_Given()
        {
            _validUser.EncryptedPassword = "secret";
            await DbContext.AddAsync(_validUser);

            var body = new LoginRequest
            {
                Username = _validUser.Email,
                Password = "not-the-password"
            };

            var response = await _controller.Login(body);
            Assert.IsType<UnauthorizedResult>(response.Result);
        }
    }
}