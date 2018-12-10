using HaruGaKita.Application.Accounts.Commands;
using HaruGaKita.Application.Accounts.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Server.Kestrel.Core;

#pragma warning disable 1591
namespace HaruGaKita.WebAPI.Controllers
{
    public class AccountsController : BaseController
    {
        public AccountsController(IMediator mediator) : base(mediator)
        { }

        /// <summary>
        /// Creates a new account
        /// </summary>
        /// <param name="createAccountRequest">Parameters needed for creating anew account</param>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UserDto>> Post([FromBody] CreateAccountCommand createAccountRequest)
        {
            return await _mediator.Send(createAccountRequest);
        }
    }
}