using System;
using HaruGaKita.Entities;
using HaruGaKita.Test.Support;
using Xunit;

namespace HaruGaKita.Test.Repositories
{
    public class EntityFrameworkRepositoryTest : IntegrationTest
    {
        private readonly User EntityA;
        private readonly User EntityB;
        public EntityFrameworkRepositoryTest() : base()
        {
            EntityA = Factories.UserFactory.Generate();
            EntityB = Factories.UserFactory.Generate();
        }

        [Fact]
        public async void GetByIdAsync_Returns_An_Entity_When_A_Matching_Id_Is_Passed()
        {
            await UserRepository.AddAsync(EntityA);
            var retrievedUser = await UserRepository.GetByIdAsync(EntityA.Id);

            Assert.NotNull(retrievedUser);
            Assert.Equal(retrievedUser, EntityA);
        }

        [Fact]
        public async void GetByIdAsync_Returns_Null_When_An_Invalid_Id_Is_Passed()
        {
            await UserRepository.AddAsync(EntityA);
            var retrievedUser = await UserRepository.GetByIdAsync(-1);

            Assert.Null(retrievedUser);
        }

        [Fact]
        public async void GetByGuidAsync_Returns_An_Entity_When_A_Matching_Guid_Is_Passed()
        {
            await UserRepository.AddAsync(EntityB);
            var retrievedUser = await UserRepository.GetByGuidAsync(EntityB.Uid);

            Assert.NotNull(retrievedUser);
            Assert.Equal(retrievedUser, EntityB);
        }

        [Fact]
        public async void GetByGuidAsync_Returns_Null_When_An_Invalid_Guid_Is_Passed()
        {
            await UserRepository.AddAsync(EntityA);
            var retrievedUser = await UserRepository.GetByGuidAsync(Guid.NewGuid());

            Assert.Null(retrievedUser);
        }

        [Fact]
        public async void ListAllAsync_Returns_A_List_Of_Entities()
        {
            await UserRepository.AddAsync(EntityA);
            await UserRepository.AddAsync(EntityB);

            var retrievedUsers = await UserRepository.ListAllAsync();

            Assert.Collection(retrievedUsers, item => Assert.Equal(item, EntityA),
                                              item => Assert.Equal(item, EntityB));
        }
    }
}
