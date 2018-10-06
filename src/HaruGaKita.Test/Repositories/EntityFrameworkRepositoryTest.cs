using System;
using HaruGaKita.Entities;
using HaruGaKita.Test.Support;
using Xunit;
using static HaruGaKita.Test.DataCase;

namespace HaruGaKita.Test.Repositories
{
    public class EntityFrameworkRepositoryTest : IntegrationTest
    {
        private readonly User EntityA;
        private readonly User EntityB;
        public EntityFrameworkRepositoryTest() : base(_connectionString)
        {
            EntityA = Factories.UserFactory.Generate();
            EntityB = Factories.UserFactory.Generate();
        }

        [Fact]
        public async void GetByIdAsync_Returns_An_Entity_When_A_Matching_Id_Is_Passed()
        {
            await _userRepository.AddAsync(EntityA);
            var retrievedUser = await _userRepository.GetByIdAsync(EntityA.Id);

            Assert.NotNull(retrievedUser);
            Assert.Equal(retrievedUser, EntityA);
        }

        [Fact]
        public async void GetByIdAsync_Returns_Null_When_An_Invalid_Id_Is_Passed()
        {
            await _userRepository.AddAsync(EntityA);
            var retrievedUser = await _userRepository.GetByIdAsync(-1);

            Assert.Null(retrievedUser);
        }

        [Fact]
        public async void GetByGuidAsync_Returns_An_Entity_When_A_Matching_Guid_Is_Passed()
        {
            await _userRepository.AddAsync(EntityB);
            var retrievedUser = await _userRepository.GetByGuidAsync(EntityB.Uid);

            Assert.NotNull(retrievedUser);
            Assert.Equal(retrievedUser, EntityB);
        }

        [Fact]
        public async void GetByGuidAsync_Returns_Null_When_An_Invalid_Guid_Is_Passed()
        {
            await _userRepository.AddAsync(EntityA);
            var retrievedUser = await _userRepository.GetByGuidAsync(Guid.NewGuid());

            Assert.Null(retrievedUser);
        }

        [Fact]
        public async void ListAllAsync_Returns_A_List_Of_Entities()
        {
            await _userRepository.AddAsync(EntityA);
            await _userRepository.AddAsync(EntityB);

            var retrievedUsers = await _userRepository.ListAllAsync();


            Assert.Collection(retrievedUsers, item => Assert.Equal(item, EntityA),
                                              item => Assert.Equal(item, EntityB));
        }
    }
}