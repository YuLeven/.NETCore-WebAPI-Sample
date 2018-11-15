using HaruGaKita.Domain.Entities;
using Xunit;

namespace HaruGaKita.Test.Entities
{
    public class UserTest
    {
        [Fact]
        public void Constructor_Encrypts_Provided_Password()
        {
            var user = new User("YuLeven", "foo@bar.com", "secret");

            Assert.True(PasswordWasSetToValidHash(user, "secret"));
        }

        [Fact]
        public void Setter_Encrypts_Password()
        {
            var user = new User();
            user.EncryptedPassword = "very_secret";

            Assert.True(PasswordWasSetToValidHash(user, "very_secret"));
        }

        private bool PasswordWasSetToValidHash(User user, string password) => BCrypt.Net.BCrypt.Verify(password, user.EncryptedPassword);
    }
}
