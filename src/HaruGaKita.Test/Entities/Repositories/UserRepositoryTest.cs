using HaruGaKita.Entities;
using HaruGaKita.Test.Support;
using Xunit;
using static HaruGaKita.Test.DataCase;

namespace HaruGaKita.Test.Entities.Repositories
{
    public class UserRepositoryTest : IntegrationTest
    {
        private readonly User UserA;
        private readonly User UserB;
        
        public UserRepositoryTest() : base(_connectionString)
        {
            UserA = Factories.UserFactory.Generate();
            UserB = Factories.UserFactory.Generate();
        }

        [Fact]
        public async void GetByIdAsync_Returns_A_User_When_Valid_Id_Is_Passed()
        {
            await _userRepository.AddAsync(UserA);
            var expectedUser = await _userRepository.GetByIdAsync(UserA.Id);

            Assert.NotNull(expectedUser);
            Assert.Equal(expectedUser, UserA);
        }

        [Fact]
        public async void ListAllAsync_Returns_A_List_Of_Users()
        {
            await _userRepository.AddAsync(UserA);
            await _userRepository.AddAsync(UserB);

            var retrievedUsers = await _userRepository.ListAllAsync();

            Assert.Collection(retrievedUsers, item => Assert.Equal(item, UserA),
                                              item => Assert.Equal(item, UserB));
        }
    }
}