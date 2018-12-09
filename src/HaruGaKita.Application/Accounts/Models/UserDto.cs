using HaruGaKita.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace HaruGaKita.Application.Accounts.Models
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public static Expression<Func<User, UserDto>> Projection
        {
            get
            {
                return u => new UserDto
                {
                    Id = u.Uid,
                    Username = u.Username,
                    Email = u.Email
                };
            }
        }
    }
}
