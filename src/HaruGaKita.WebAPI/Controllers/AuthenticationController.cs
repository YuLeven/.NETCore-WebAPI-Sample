using HaruGaKita.Application.Accounts.Commands;
using HaruGaKita.Application.Accounts.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Server.Kestrel.Core;

#pragma warning disable 1591
namespace HaruGaKita.WebAPI.Controllers
{
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
        [Route("/api/login")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<OAuthCredentials>> Login([FromBody] LoginCommand loginRequest)
        {
            return await _mediator.Send(loginRequest);
        }

        [HttpPost]
        [Route("/api/accounts/new")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserDto>> CreateAccount([FromBody] CreateAccountCommand request)
        {
            return await _mediator.Send(request);
        }
    }
}