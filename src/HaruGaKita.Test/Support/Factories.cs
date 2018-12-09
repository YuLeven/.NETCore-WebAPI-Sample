using Bogus;
using HaruGaKita.Domain.Entities;

namespace HaruGaKita.Test.Support
{
    public static class Factories
    {
        public static Faker<User> UserFactory
        {
            get
            {
                return new Faker<User>()
                                .RuleFor(o => o.Email, f => f.Internet.Email())
                                .RuleFor(o => o.EncryptedPassword, f => f.Internet.Password())
                                .RuleFor(o => o.Username, f => f.Internet.UserName());
            }
        }
    }
}