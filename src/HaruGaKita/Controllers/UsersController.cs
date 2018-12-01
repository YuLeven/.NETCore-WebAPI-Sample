using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using HaruGaKita.Application.Accounts.Queries;
using HaruGaKita.Application.Accounts.Models;

#pragma warning disable 1591
namespace HaruGaKita.WebAPI.Controllers
{
    [Route("api/me")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves the current authenticated user
        /// </summary>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto>> Me()
        {
            var query = new MeQuery(User);
            return await _mediator.Send(query);
        }
    }
}
