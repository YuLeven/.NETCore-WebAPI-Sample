using HaruGaKita.Domain.Entities;
using Newtonsoft.Json;
using Xunit;

namespace HaruGaKita.Test.Entities.Serialization
{
    public class UserJsonSerializationTest
    {
        private readonly User _user;

        public UserJsonSerializationTest()
        {
            _user = new User();
        }

        [Fact]
        public void Serializing_User_Excludes_Sensitive_Fields()
        {
            var serializedUser = JsonConvert.SerializeObject(_user);

            Assert.DoesNotContain("EncryptedPassword", serializedUser);
        }
    }
}