using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using HaruGaKita.Domain.Exceptions;
using HaruGaKita.Test.Support;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace HaruGaKita.Test.Service
{
    public class UserServiceTest : IntegrationTest
    {
        [Fact]
        public async void AuthenticateUser_Returns_An_Token_And_User_When_Valid_Credentials_Are_Provided()
        {
            var user = Factories.UserFactory.Generate();
            user.EncryptedPassword = "secret";
            await DbContext.AddAsync(user);
            var authenticatedUser = await UserService.AuthenticateUser(user.Email, "secret");

            Assert.Equal(user, authenticatedUser);
        }

        [Fact]
        public async void AuthenticateUser_Throws_When_Invalid_Credentials_are_Provided()
        {
            var user = Factories.UserFactory.Generate();
            user.EncryptedPassword = "secret";
            await DbContext.AddAsync(user);

            await Assert.ThrowsAsync<UnauthenticatedException>(async () =>
             {
                 await UserService.AuthenticateUser(user.Email, "not_so_secret");
             });
        }

        [Fact]
        public async void AuthenticateUser_Throws_When_No_User_Exists()
        {
            var user = Factories.UserFactory.Generate();
            user.EncryptedPassword = "secret";
            await DbContext.AddAsync(user);

            await Assert.ThrowsAsync<UnauthenticatedException>(async () =>
             {
                 await UserService.AuthenticateUser(user.Email, "not_so_secret");
             });
        }

        [Fact]
        public async void SignCredentials_Throws_When_Wrong_Password_Is_Provided()
        {
            var user = Factories.UserFactory.Generate();
            user.EncryptedPassword = "secret";
            await DbContext.AddAsync(user);

            await Assert.ThrowsAsync<UnauthenticatedException>(async () =>
            {
                await UserService.AuthenticateUser(user.Email, "not_so_secret");
            });
        }

        [Fact]
        public async void SignCredentials_Throws_When_No_User_Exists()
        {
            await Assert.ThrowsAsync<UnauthenticatedException>(async () =>
            {
                await UserService.SignCredentials("not@existent.com", "not_so_secret");
            });
        }

        [Fact]
        public async void SignCredentials_Returns_A_Valid_Jwt_When_Given_Valid_Credentials()
        {
            var user = Factories.UserFactory.Generate();
            user.EncryptedPassword = "secret";
            await DbContext.AddAsync(user);

            var token = await UserService.SignCredentials(user.Email, "secret");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadJwtToken(token);
            var validationParameters = new TokenValidationParameters
            {
                ValidAudience = Common.Configuration.ApiAudience,
                ValidIssuer = Common.Configuration.AppAuthority,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = Common.Configuration.ApplicationSecurityKey
            };
            SecurityToken validatedToken;
            tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

            var sub = jsonToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub);
            var expireAt = jsonToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Exp);
            var nowEpoch = DateTimeOffset.Now.ToUnixTimeSeconds();

            Assert.NotNull(token);
            Assert.NotNull(validatedToken);
            Assert.Equal(sub.Value, user.Uid.ToString());
            Assert.InRange(Convert.ToInt32(expireAt.Value), nowEpoch - 10_000, nowEpoch + 10_000);
        }

        [Fact]
        public async void GetCurrentUser_Returns_An_User_When_A_Valid_Claim_Is_Passed()
        {
            var user = Factories.UserFactory.Generate();
            await DbContext.AddAsync(user);
            var claims = new TestPrincipal(new Claim(JwtRegisteredClaimNames.Sub, user.Uid.ToString()));
            var currentUser = await UserService.GetCurrentUser(claims);

            Assert.Equal(user, currentUser);
        }

        [Fact]
        public async void GetCurrentUser_Returns_Null_When_Invalid_Claim_Is_Passed()
        {
            var claims = new TestPrincipal(new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()));
            var currentUser = await UserService.GetCurrentUser(claims);

            Assert.Null(currentUser);
        }

        [Fact]
        public async void GetCurrentUser_Throws_When_Invalid_Guid_Is_Passed()
        {
            var claims = new TestPrincipal(new Claim(JwtRegisteredClaimNames.Sub, "not-a-valid-guid. Really."));

            await Assert.ThrowsAsync<FormatException>(async () =>
            {
                await UserService.GetCurrentUser(claims);
            });
        }
    }

}
