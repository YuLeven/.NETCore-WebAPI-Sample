using HaruGaKita.Application.Accounts.Commands;
using HaruGaKita.Application.Accounts.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Server.Kestrel.Core;

#pragma warning disable 1591
namespace HaruGaKita.WebAPI.Controllers
{
    public class SessionsController : BaseController
    {
        public SessionsController(IMediator mediator) : base(mediator)
        { }

        /// <summary>
        /// Signs-in an user
        /// </summary>
        /// <param name="loginRequest">Houses the username and password request</param>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<OAuthCredentials>> Post([FromBody] LoginCommand loginRequest)
        {
            return await _mediator.Send(loginRequest);
        }
    }
}