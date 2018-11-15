using Xunit;
using HaruGaKita.WebAPI.Controllers;
using HaruGaKita.Test.Support;
using HaruGaKita.Domain.Entities;

namespace HaruGaKita.Test.Controllers
{
    public class UsersControllerTest : IntegrationTest
    {
        private UsersController _controller;
        private readonly User _expectedReturn;

        public UsersControllerTest() : base()
        {
            _expectedReturn = Factories.UserFactory.Generate();
            DbContext.AddAsync(_expectedReturn).GetAwaiter().GetResult();
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
