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
    }
}