using System.Threading.Tasks;
using HaruGaKita.Test.Support;
using HaruGaKita.Application.Accounts.Commands;
using Xunit;
using System.Threading;
using HaruGaKita.Application.Accounts.Models;
using HaruGaKita.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;

namespace HaruGaKita.Test.Data.Entities
{
    public class UserTest : IntegrationTest
    {
        [Fact]
        public async Task Fails_When_Email_Is_Already_Taken()
        {
            var existingUser = await DbContext.Users.AddAsync(Factories.UserFactory.Generate());
            var user = new User
            {
                Email = existingUser.Entity.Email
            };

            await DbContext.Users.AddAsync(user);

            await Assert.ThrowsAsync<DbUpdateException>(async () =>
            {
                await DbContext.SaveChangesAsync();
            });
        }
    }
}