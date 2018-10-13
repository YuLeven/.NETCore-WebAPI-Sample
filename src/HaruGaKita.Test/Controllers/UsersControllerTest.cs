using Xunit;
using HaruGaKita.Controllers;
using HaruGaKita.Infrastructure.Interfaces;
using Moq;
using HaruGaKita.Entities;
using HaruGaKita.Test.Support;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace HaruGaKita.Test.Controllers
{
    public class UsersControllerTest : IntegrationTest
    {
        private UsersController _controller;
        private readonly User _expectedReturn;

        public UsersControllerTest() : base()
        {
            _expectedReturn = Factories.UserFactory.Generate();
            UserRepository.AddAsync(_expectedReturn).GetAwaiter().GetResult();
            _controller = new UsersController(UserService);
            UseAuthenticatedUser(_controller, _expectedReturn);
        }

        [Fact]
        public async void Me_Returns_The_Current_Authenticated_User()
        {
            var result = await _controller.Me();
            Assert.Equal(result.Value, _expectedReturn);
        }
    }
}
