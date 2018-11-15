using System.Threading.Tasks;
using HaruGaKita.Domain.Exceptions;
using HaruGaKita.Models;
using HaruGaKita.Services;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable 1591
namespace HaruGaKita.WebAPI.Controllers
{
    [Route("/api/login")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Signs-in an user
        /// </summary>
        /// <param name="loginRequest">Houses the username and password request</param>
        /// <returns code="201">A JWT token</returns>
        /// <returns code="401">If the credentials are invalid</returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<OAuthCredentials>> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                return new OAuthCredentials
                {
                    Token = await _userService.SignCredentials(loginRequest.Username, loginRequest.Password)
                };
            }
            catch (UnauthenticatedException)
            {
                return Unauthorized();
            }
        }
    }
}