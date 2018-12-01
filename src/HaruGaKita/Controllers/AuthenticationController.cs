using HaruGaKita.Application.Accounts.Commands;
using HaruGaKita.Application.Accounts.Models;
using HaruGaKita.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;

#pragma warning disable 1591
namespace HaruGaKita.WebAPI.Controllers
{
    [Route("/api/login")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
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
        public async Task<ActionResult<OAuthCredentials>> Login([FromBody] LoginCommand loginRequest)
        {
            try
            {
                return await _mediator.Send(loginRequest);
            }
            catch (UnauthenticatedException)
            {
                return Unauthorized();
            }
        }
    }
}