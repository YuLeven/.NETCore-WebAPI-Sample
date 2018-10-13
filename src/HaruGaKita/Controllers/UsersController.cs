using System;
using System.Security.Claims;
using System.Threading.Tasks;
using HaruGaKita.Entities;
using HaruGaKita.Infrastructure.Interfaces;
using HaruGaKita.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using HaruGaKita.Services;

#pragma warning disable 1591
namespace HaruGaKita.Controllers
{
    [Route("api/me")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        /// <summary>
        /// Retrieves the current authenticated user
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<User>> Me()
        {
            return await _userService.GetCurrentUser(User);
        }
    }
}
