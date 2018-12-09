#pragma warning disable 1591
using HaruGaKita.Application.Accounts.Models;
using MediatR;

namespace HaruGaKita.Application.Accounts.Commands
{
    public class CreateAccountCommand : IRequest<UserDto>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmation { get; set; }
    }
}