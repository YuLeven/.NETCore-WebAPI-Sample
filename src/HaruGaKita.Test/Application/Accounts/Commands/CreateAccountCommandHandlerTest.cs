using System.Threading.Tasks;
using HaruGaKita.Test.Support;
using HaruGaKita.Application.Accounts.Commands;
using Xunit;
using System.Threading;
using HaruGaKita.Application.Accounts.Models;
using HaruGaKita.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HaruGaKita.Test.Application.Accounts.Commands
{
    public class CreateAccountCommandHandlerTest : IntegrationTest
    {
        [Fact]
        public async Task CreateNewAccount()
        {
            var handler = new CreateAccountCommandHandler(DbContext);
            var request = new CreateAccountCommand
            {
                Email = "yurileven@gmail.com",
                Password = "AUDd9d722h(H!U&(H!wbu13g",
                PasswordConfirmation = "AUDd9d722h(H!U&(H!wbu13g"
            };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.IsType(typeof(UserDto), result);
            Assert.Equal("yurileven@gmail.com", result.Email);

            var createdEntry = await DbContext
                                        .Set<User>()
                                        .Where(u => u.Uid == result.Id)
                                        .FirstAsync();

            Assert.NotNull(createdEntry);
            Assert.True(BCrypt.Net.BCrypt.Verify("AUDd9d722h(H!U&(H!wbu13g", createdEntry.EncryptedPassword));
        }
    }
}