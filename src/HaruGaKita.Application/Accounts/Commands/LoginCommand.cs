#pragma warning disable 1591
using HaruGaKita.Application.Accounts.Models;
using MediatR;

namespace HaruGaKita.Application.Accounts.Commands
{
    public class LoginCommand : IRequest<OAuthCredentials>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}