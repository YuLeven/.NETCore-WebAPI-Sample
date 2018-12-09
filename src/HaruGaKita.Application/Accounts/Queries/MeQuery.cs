using HaruGaKita.Application.Accounts.Models;
using MediatR;
using System;
using System.Security.Claims;

namespace HaruGaKita.Application.Accounts.Queries
{
    public class MeQuery : IRequest<UserDto>
    {
        public ClaimsPrincipal User;

        public MeQuery(ClaimsPrincipal user)
        {
            User = user;
        }
    }
}
