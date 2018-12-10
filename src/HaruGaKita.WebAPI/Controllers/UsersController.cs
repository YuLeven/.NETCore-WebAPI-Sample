using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using HaruGaKita.Application.Accounts.Queries;
using HaruGaKita.Application.Accounts.Models;

#pragma warning disable 1591
namespace HaruGaKita.WebAPI.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Retrieves the current authenticated user
        /// </summary>
        [HttpGet]
        [Authorize]
        [Route("me")]
        public async Task<ActionResult<UserDto>> Me()
        {
            var query = new MeQuery(User);
            return await _mediator.Send(query);
        }
    }
}
