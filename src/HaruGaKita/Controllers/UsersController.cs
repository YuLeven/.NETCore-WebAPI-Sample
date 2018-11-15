using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HaruGaKita.Services;
using HaruGaKita.Domain.Entities;

#pragma warning disable 1591
namespace HaruGaKita.WebAPI.Controllers
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
