#pragma warning disable 1591
namespace HaruGaKita.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string EncryptedPassword;

        public User() { }

        public User(string username, string email, string password)
        {
            Username = username;
            Email = email;
            EncryptedPassword = password;
        }
    }
}
